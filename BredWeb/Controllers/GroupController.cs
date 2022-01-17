using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BredWeb.Controllers
{
    public class GroupController : Controller
    {
        private readonly UserManager<Person> userManager;
        private readonly SignInManager<Person> signInManager;
        private readonly ApplicationDbContext _db;

        public GroupController(UserManager<Person> userManager,
                                 SignInManager<Person> signInManager,
                                 ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Group> objGroupList = _db.Groups;
            return View(objGroupList);
        }

        //GET
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var user = await userManager.GetUserAsync(User);
            Console.WriteLine(user.NickName);
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Group obj)
        {
            var user = await userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                obj.StartDate = DateTime.Now;
                obj.Creator = user.NickName;
                _db.Groups.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Group created successfully";
                return RedirectToAction("Index"); //goes to this controllers "index", to go to a different controller use ("action", "controllerName")
            }
            return View(obj);
        }
    }
}
