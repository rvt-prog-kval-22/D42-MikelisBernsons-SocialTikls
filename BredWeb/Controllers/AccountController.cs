using BredWeb.Data;
using BredWeb.Interfaces;
using BredWeb.Models;
using BredWeb.Services;
using FluentEmail.Core;
using FluentEmail.Razor;
using FluentEmail.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BredWeb.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<Person> userManager;
        private readonly SignInManager<Person> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext db;
        private readonly IAccountService _account;

        public AccountController(UserManager<Person> userManager,
                                 SignInManager<Person> signInManager,
                                 IConfiguration configuration,
                                 ApplicationDbContext db,
                                 IAccountService accountService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this._account = accountService;
            this.db = db;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(_account.GetAccountViewModel(await userManager.GetUserAsync(User)));
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
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

                var users = db.Users;
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
                        var result = await userManager.CreateAsync(user, obj.Password);

                        if (result.Succeeded)
                        {
                            await userManager.ConfirmEmailAsync(user, await userManager.GenerateEmailConfirmationTokenAsync(user));
                            await signInManager.SignInAsync(user, isPersistent: false);
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
                var result = await signInManager.PasswordSignInAsync(obj.Email, obj.Password, obj.RememberMe, false);

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
            await SendEmail(receiver, "Test body.");
            return RedirectToAction("Index", "Account");
        }

        private async Task<IActionResult> SendEmail(string receiver, string body)
        {
            string? env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var fromEmail = configuration.GetValue<string>($"EmailStrings:{env}:From");
            string client = configuration.GetValue<string>($"EmailStrings:{env}:Client");
            int port = configuration.GetValue<int>($"EmailStrings:{env}:Port");
            string senderEmail = configuration.GetValue<string>($"EmailStrings:{env}:SenderEmail");
            string senderPassword = configuration.GetValue<string>($"EmailStrings:{env}:SenderPassword");

            var sender = new SmtpSender();

            if (env == "Production")
            {
                sender = new SmtpSender(() => new SmtpClient(client)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = port,
                    Credentials = new NetworkCredential(senderEmail, senderPassword)
                });
            }
            else
            {
                sender = new SmtpSender(() => new SmtpClient(client)
                {
                    UseDefaultCredentials = false,
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = port
                });
            }

            Email.DefaultSender = sender;
            Email.DefaultRenderer = new RazorRenderer();

            StringBuilder template = new();
            template.AppendLine("Hi," + "@Model.Name");
            template.AppendLine($"<p>{body}</p>");
            template.AppendLine("- Breddit");

            var email = Email
                .From(fromEmail)
                .To(receiver)
                .Subject("Email from breddit :o")
                .UsingTemplate(template.ToString(), new { Name = "" });

            var result = await email.SendAsync();

            foreach (var error in result.ErrorMessages)
            {
                Console.WriteLine(error);
            }

            return new EmptyResult();
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
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                    var confirmationToken = await userManager.GeneratePasswordResetTokenAsync(user);

                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                        new {email = model.Email, token = confirmationToken }, Request.Scheme);

                    await SendEmail(user.Email, "This is a link to reset your password in Breddit.\nIf you did not make this request it is safe to ignore it.\n\n" + passwordResetLink);

                    return View("ForgotPasswordConfirmation");
                }
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
                var user = await userManager.FindByEmailAsync(model.Email);
                if(user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
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
