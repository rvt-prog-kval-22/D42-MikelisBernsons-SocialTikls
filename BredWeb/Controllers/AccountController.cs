using BredWeb.Data;
using BredWeb.Interfaces;
using BredWeb.Models;
using BredWeb.Services;
//using FluentEmail.Core;
//using FluentEmail.Razor;
//using FluentEmail.Smtp;

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using MailKit;
using MailKit.Net.Smtp;
using MimeKit;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;
using MailKit.Security;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace BredWeb.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;
        private readonly IAccountService _account;

        public AccountController(UserManager<Person> userManager,
                                 SignInManager<Person> signInManager,
                                 IConfiguration configuration,
                                 ApplicationDbContext db,
                                 IAccountService accountService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
            this._account = accountService;
            this._db = db;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(_account.GetAccountViewModel(await _userManager.GetUserAsync(User)));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel obj)
        {
            if (ModelState.IsValid && obj != null)
            {

                var users = _db.Users;
                if(users.FirstOrDefault(u => u.Email.Equals(obj.Email)) == null)
                {
                    if (users.FirstOrDefault(u => u.NickName!.Equals(obj.NickName)) == null)
                    {
                        var user = new Person
                        {
                            UserName = obj.Email,
                            Email = obj.Email,
                            NickName = obj.NickName,
                            BirthDay = obj.BirthDay,
                            DateCreated = DateTime.Now
                        };

                        if(user.BirthDay > DateTime.Now)
                        {
                            user.BirthDay = DateTime.Now;
                        }
                        var result = await _userManager.CreateAsync(user, obj.Password);

                        if (result.Succeeded)
                        {
                            await _userManager.ConfirmEmailAsync(user, await _userManager.GenerateEmailConfirmationTokenAsync(user));
                            await _signInManager.SignInAsync(user, isPersistent: false);
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", "NickName is already in use");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email is already in use");
                }
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel obj)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(obj.Email, obj.Password, obj.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid Login details");
            }

            return View(obj);
        }

        public async Task<IActionResult> ConfirmAccount(string receiver)
        {
            await SendEmailAsync(receiver, "Test body.");
            return RedirectToAction("Index", "Account");
        }

        private async Task<IActionResult> SendEmailAsync(string receiver, string body)
        {
            string clientProvider = "smtp.gmail.com";
            int port = 587;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Breddit", "breddittest@gmail.com"));
            message.To.Add(new MailboxAddress("User?", receiver));
            message.Subject = "Breddit password change";
            message.Body = new TextPart("plain") { Text = body };

            using (var client = new SmtpClient())
            {
                var secretClient = new SecretClient(new Uri(Environment.GetEnvironmentVariable("VaultUri")), new DefaultAzureCredential());
                KeyVaultSecret BredditSenderEmail = await secretClient.GetSecretAsync("BredditSenderEmail");
                KeyVaultSecret BredditSenderPassword = await secretClient.GetSecretAsync("BredditSenderPassword");
                await client.ConnectAsync(clientProvider, port, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(BredditSenderEmail.Value.ToString(), BredditSenderPassword.Value.ToString());
                await client.SendAsync(message);
                client.Disconnect(true);
            }

            return View("Index");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null && await _userManager.IsEmailConfirmedAsync(user))
                {
                    var confirmationToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                        new {email = model.Email, token = confirmationToken }, Request.Scheme);

                    await SendEmailAsync(user.Email, "This is a link to reset your password in Breddit.\nIf you did not make this request it is safe to ignore it.\n\n" + passwordResetLink);

                    return View("ForgotPasswordConfirmation");
                }
                ViewData["Title"] = "Email Sent.";
                return View("ForgotPasswordConfirmation");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if(token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid token");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }
    }
}
