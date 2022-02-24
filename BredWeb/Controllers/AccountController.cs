using BredWeb.Data;
using BredWeb.Models;
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

        public AccountController(UserManager<Person> userManager,
                                 SignInManager<Person> signInManager,
                                 IConfiguration configuration,
                                 ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.db = db;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);
            return View(user);
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
        public async Task<IActionResult> Register(Register obj)
        {
            if (ModelState.IsValid)
            {

                var users = db.Users;
                if(users.FirstOrDefault(u => u.Email.Equals(obj.Email)) == null)
                {
                    if (users.FirstOrDefault(u => u.NickName.Equals(obj.NickName)) == null)
                    {
                        var user = new Person
                        {
                            UserName = obj.Email,
                            Email = obj.Email,
                            NickName = obj.NickName,
                            BirthDay = obj.BirthDay,
                            DateCreated = DateTime.Now
                        };

                        //var confirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                        var result = await userManager.CreateAsync(user, obj.Password);

                        if (result.Succeeded)
                        {
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
        public async Task<IActionResult> Login(Login obj)
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
            template.AppendLine("Hi @Model.Name,");
            template.AppendLine($"<p>{body}</p>");
            template.AppendLine("- Breddit");

            var email = Email
                .From(fromEmail)
                .To(receiver)
                .Subject("Email from breddit :o")
                .UsingTemplate(template.ToString(), new { Name = (await userManager.GetUserAsync(User)).NickName });

            var result = await email.SendAsync();

            foreach (var error in result.ErrorMessages)
            {
                Console.WriteLine(error);
            }

            return new EmptyResult();
        }
    }
}
