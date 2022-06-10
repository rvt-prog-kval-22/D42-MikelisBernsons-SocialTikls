using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BredWeb.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AdminController(UserManager<Person> userManager,
                                 SignInManager<Person> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 IConfiguration configuration,
                                 ApplicationDbContext db)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
            _db = db;
        }

        //GET
        public IActionResult Index()
        {
            return View();
        }

        //GET
        [AllowAnonymous]
        public async Task<IActionResult> Setup()
        {
            IList<Person>? users = await _userManager.GetUsersInRoleAsync("Admin");
            if(users.Count > 0)
            {
                ViewBag.ErrorMessage = "An admin already exists.";
                return View("Error");
            }

            string settingsEmail = _configuration.GetValue<string>($"AdminSetup:Email");

            Person? admin = await _userManager.FindByEmailAsync(settingsEmail);
            if (admin != null)
            {
                ViewBag.ErrorMessage = "A user with the specified email already exists.\n" + 
                                       "Please update the AdminSetup section.";
                return View("Error");
            }

            if(settingsEmail == "")
            {
                ViewBag.ErrorMessage = "AdminSetup empty.";
                return View("Error");
            }

            string settingsNickName = _configuration.GetValue<string>($"AdminSetup:NickName");
            string settingsPassword = _configuration.GetValue<string>($"AdminSetup:Password");

            var user = new Person
            {
                UserName = settingsEmail,
                Email = settingsEmail,
                NickName = settingsNickName,
                BirthDay = DateTime.Now,
                DateCreated = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, settingsPassword);
            if (result.Succeeded)
            {
                if (await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    await _roleManager.CreateAsync(new IdentityRole { Name = "Admin"});
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                ViewBag.ErrorMessage = "Admin account created successfuly\n"+
                                       "You can now log in.";
                return View("Error");
            }
            else
            {
                ViewBag.ErrorMessage = "Admin account creation failed.\n"+
                                       "Please check the AdminSetup section.";
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole()
        {
            var role = await _roleManager.FindByNameAsync("Admin");

            if(role == null)
            {
                ViewBag.ErrorMessage = "Role not found";
                return View("Error");
            }

            ViewBag.RoleId = role.Id;
            List<UserInRoleViewModel> model = new();
            var adminList = await _userManager.GetUsersInRoleAsync("Admin");

            foreach(var admin in adminList)
            {
                model.Add(new UserInRoleViewModel() {UserId = admin.Id, UserName = admin.NickName!, IsSelected = false });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserInRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role [{roleId}] was not found";
                return View("Error");
            }

            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
            if(usersInRole.Count <= 1)
            {
                TempData["Error"] = "Can't remove all admins";
                return RedirectToAction("EditUsersInRole", new { roleId = roleId });
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);
                usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);
                if (usersInRole.Count > 1)
                {
                    if(model[i].IsSelected)
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    TempData["Error"] = "Can't remove all admins, the last admin in the list was not removed";
                    break;
                }
            }

            return RedirectToAction("EditUsersInRole", new { roleId = roleId });
        }

        [HttpPost]
        public async Task<IActionResult> AddToRole(string email, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var user = _db.People.FirstOrDefault(x => x.Email == email);

            if (role == null || user == null)
            {
                ViewBag.ErrorMessage = $"Role or User was not found";
                return View("Error");
            }

            await _userManager.AddToRoleAsync(user, role.Name);

            return RedirectToAction("EditUsersInRole", new { roleId = roleId });
        }
    }
}
