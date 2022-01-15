using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BredWeb.Controllers
{
    public class GroupController : Controller
    {
        private readonly ApplicationDbContext _db;

        public GroupController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Group> objGroupList = _db.Groups;
            return View(objGroupList);
        }

        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Group obj)
        {
            if (ModelState.IsValid)
            {
                _db.Groups.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Group created successfully";
                return RedirectToAction("Index"); //goes to this controllers "index", to go to a different controller use ("action", "controllerName")
            }
            return View(obj);
        }
    }
}
