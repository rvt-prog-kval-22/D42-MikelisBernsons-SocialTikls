using BredWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BredWeb.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<Person> userManager;
        private readonly SignInManager<Person> signInManager;

        public AccountController(UserManager<Person> userManager,
                                 SignInManager<Person> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
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
                var user = new Person
                {
                    UserName = obj.Email,
                    Email = obj.Email,
                    NickName = obj.NickName,
                    BirthDay = obj.BirthDay
                };
                var result = await userManager.CreateAsync(user, obj.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
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
    }
}
