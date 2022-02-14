using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BredWeb.Controllers
{
    public class GroupController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public GroupController(UserManager<Person> userManager,
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
        public async Task<IActionResult> Index()
        {
            ViewBag.nick = "";
            if (_signInManager.IsSignedIn(User))
            {
                ViewBag.nick = (await _userManager.GetUserAsync(User)).NickName;
            }
            IEnumerable<Group> objGroupList = _db.Groups;
            return View(objGroupList);
        }

        //GET
        [Authorize]
        public IActionResult Create()
        {            
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(Group obj)
        {
            var user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                if(_db.Groups.Any(g => g.Title == obj.Title))
                {
                    ModelState.AddModelError("CustomError",obj.Title + " already exists.");
                }
                else
                {
                    obj.StartDate = DateTime.Now;
                    obj.Creator = user.NickName;
                    //obj.UserIdList.Add(new UserIdList { GroupId = obj.Id, PersonId = user.Id});
                    //obj.AdminIdList.Add(Int32.Parse(user.Id));
                    obj.UserList.Add(user);
                    obj.UserCount++;
                    _db.Groups.Add(obj);
                    _db.SaveChanges();
                    TempData["success"] = "Group created successfully";
                    return RedirectToAction("Index"); //goes to this controllers "index", to go to a different controller use ("action", "controllerName")
                }
            }
            return View(obj);
        }

        //GET
        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var groupFromDb = _db.Groups.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if (groupFromDb == null)
                return NotFound();

            return View(groupFromDb);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult DeleteGroup(int? id)
        {
            var obj = _db.Groups.Find(id);
            if (obj == null)
                return NotFound();

            _db.Posts.RemoveRange(_db.Posts.Where(p => p.GroupId == id));

            _db.Groups.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Group deleted successfully";
            return RedirectToAction("Index"); //goes to this controllers "index", to go to a different controller use ("action", "controllerName")

        }

        //GET
        [Authorize]
        public async Task<IActionResult> Join(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var group = _db.Groups.Find(id);
            var user = await _userManager.GetUserAsync(User);

            if (group == null)
                return NotFound();

            try
            {
                group.UserList.Add(user);
                group.UserCount++;
                _db.Groups.Update(group);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            //group.UserList.Add(user);
            //group.UserCount++;
            //_db.Groups.Add(group);
            //_db.SaveChanges();
            TempData["success"] = "Group Joined successfully";

            return RedirectToAction("BrowseGroup", "Post", new { id = group.Id });
        }

        //GET
        public IActionResult Open(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var groupFromDb = _db.Groups.Find(id);

            if (groupFromDb == null)
                return NotFound();

            //return View("Browse", groupFromDb);
            return RedirectToAction("BrowseGroup", "Post", new { id = groupFromDb.Id });
        }

        //GET
        public IActionResult Search(string substr)
        {
            if(substr != null && substr != "")
            {
                var groups = _db.Groups;
                var result = groups.Where(g => g.Title.Contains(substr))
                    .ToList();

                return View("Index", result);
            }
            return View("Index", _db.Groups.ToList());
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id is null or 0)
                return NotFound();
            var group = _db.Groups.Find(id);
            //var categoryFromDbFirst = _db.Categories.FirstOrDefault(u => u.Id == id);
            //var categoryFromDbSingle = _db.Categories.SingleOrDefault(u => u.Id == id);

            if(group == null)
                return NotFound();

            return View(group);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Group obj)
        {
            Group group = _db.Groups.Find(obj.Id);
            group.Description = obj.Description;

            if (ModelState.IsValid)
            {
                _db.Groups.Update(group);
                _db.SaveChanges();
                TempData["success"] = "Success";
                return RedirectToAction("BrowseGroup", "Post", new { id = obj.Id });
            }
            return View(obj);
        }
    }
}
