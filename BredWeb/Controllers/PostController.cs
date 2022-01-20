using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BredWeb.Controllers
{
    public class PostController : Controller
    {
        private readonly UserManager<Person> userManager;
        private readonly SignInManager<Person> signInManager;
        private readonly ApplicationDbContext _db;

        public PostController(UserManager<Person> userManager,
                                 SignInManager<Person> signInManager,
                                 ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _db = db;
        }

        //GET
        public IActionResult Index()
        {
            return View();
        }

        //POST
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        public IActionResult Create(Post post, int id)
        {

            if (id == null || id == 0)
                return NotFound();
            var group = _db.Groups.Find(id);

            if (group == null)
                return NotFound();


            try
            {
                group.Posts.Add(post);
                // post counter ?
                _db.Groups.Update(group);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            

            _db.SaveChanges();
            TempData["success"] = "Group deleted successfully";
            return RedirectToAction("Index"); //goes to this controllers "index", to go to a different controller use ("action", "controllerName")


            return View();
        }
    }
}
