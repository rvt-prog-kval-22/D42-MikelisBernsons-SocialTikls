using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BredWeb.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<Person> userManager,
                                 SignInManager<Person> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 ApplicationDbContext db)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            _db = db;
        }

        //GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(Role obj)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = obj.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    TempData["success"] = "Success";
                    return RedirectToAction("ListRoles", "Admin");
                }
            }
            return View(obj);
        }

        //GET
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role [{id}] was not found";
                return View("Error");
            }

            EditRole model = new()
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach(var user in _userManager.Users)
            {
                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            if(model == null)
            {
                ViewBag.ErrorMessage = $"Role [{id}] had no associated users";
                return View("Error");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRole model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role [{role.Id}] was not found";
                return View("Error");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.RoleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role [{roleId}] was not found";
                return View("Error");
            }

            List<UserInRole> model = new();

            foreach (var user in _userManager.Users)
            {
                UserInRole userInRole = new()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                userInRole.IsSelected = await _userManager.IsInRoleAsync(user, role.Name);

                model.Add(userInRole);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserInRole> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if(role == null)
            {
                ViewBag.ErrorMessage = $"Role [{roleId}] was not found";
                return View("Error");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult? result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if(i < model.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        return RedirectToAction("EditRole", new { Id = roleId });
                    }
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}
