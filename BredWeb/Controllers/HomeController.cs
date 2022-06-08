using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Diagnostics;

namespace BredWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ApplicationDbContext _db;

        public HomeController(UserManager<Person> userManager,
                              SignInManager<Person> signInManager,
                              ApplicationDbContext db)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            _db = db;
        }

        //GET
        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                Person user = await _userManager.GetUserAsync(User);
                HomeFeedViewModel model = new();
                List<Post> posts = new();
                model.Groups = _db.Groups.Where(g => g.UserList!.Contains(user)).ToList();
                if (model.Groups.Count > 0)
                {
                    foreach(var group in model.Groups)
                    {
                        _db.Entry(group).Collection(g => g.Posts!).Load();
                        if(group.Posts!.Count > 0)
                            posts.AddRange(group.Posts);
                    }
                }
                model.Posts = posts.OrderByDescending(p => p.TotalRating).ToList();
                return View(model);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}