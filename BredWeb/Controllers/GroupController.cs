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
            foreach (var group in objGroupList)
            {
                _db.Entry(group).Collection(g => g.AdminList).Load();
            }
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
                    obj.UserList.Add(user);
                    obj.AdminList.Add(new Admin { AdminId = user.Id, Email = user.Email, UserName = user.NickName });
                    obj.UserCount++;
                    _db.Groups.Add(obj);
                    _db.SaveChanges();
                    TempData["success"] = "Group created successfully";
                    return RedirectToAction("Index");
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

            foreach (var post in _db.Posts.Where(p => p.GroupId == id))
            {
                _db.Ratings.RemoveRange(_db.Ratings.Where(r => r.RatedItemId == post.Id));
            }

            _db.Posts.RemoveRange(_db.Posts.Where(p => p.GroupId == id));
            _db.Entry(obj).Collection(g => g.AdminList).Load();
            obj.AdminList.Clear();
            _db.Groups.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Group deleted successfully";
            return RedirectToAction("Index");

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
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id is null or 0)
                return NotFound();
            var group = _db.Groups.Find(id);

            if(group == null)
                return NotFound();
            _db.Entry(group).Collection(g => g.AdminList).Load();
            return View(group);
        }

        //POST
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult EditDescription(string Title, string Description, int Id)
        {
            Group group = _db.Groups.Find(Id);

            if (ModelState.IsValid && group != null)
            {
                group.Description = Description;
                _db.Groups.Update(group);
                _db.SaveChanges();
                TempData["success"] = "Success";
                return RedirectToAction("BrowseGroup", "Post", new { id = Id });
            }

            return RedirectToAction("Index", "Group");
        }

        //POST
        [HttpPost]
        [Authorize]
        [ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveAdmins(Group obj)
        {
            Group dbGroup = _db.Groups.Find(obj.Id);
            _db.Entry(dbGroup).Collection(g => g.AdminList).Load();

            if (dbGroup != null)
            {
                if(dbGroup.AdminList.Count <= 1)
                {
                    TempData["Error"] = "Can't remove all admins";
                    return RedirectToAction("Edit", new { id = obj.Id });
                }
                foreach (var admin in obj.AdminList)
                {
                    if(dbGroup.AdminList.Count > 1)
                    {
                        if (admin.IsSelected)
                        {
                            dbGroup.AdminList.Remove(dbGroup.AdminList.Find(x => x.AdminId == admin.AdminId));
                        }
                    }
                    else
                    {
                        TempData["Error"] = "All admins were removed except the last one in the list";
                        break;
                    }
                }              

                _db.SaveChanges();
                return RedirectToAction("Edit", new { id = obj.Id });
            }
            TempData["Error"] = "Group was null";
            return RedirectToAction("Edit", new { id = obj.Id });
        }

        //POST
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult AddAdmin(string email, int Id)
        {
            Group? group = _db.Groups.Find(Id);

            if (ModelState.IsValid && group != null)
            {

                Person? newAdmin = _db.People.FirstOrDefault(p => p.Email == email);
                _db.Entry(group).Collection(g => g.AdminList).Load();

                foreach(Admin admin in group.AdminList)
                {
                    if(admin.Email == email)
                    {
                        TempData["Error"] = "Add admin failed";
                        return RedirectToAction("Edit", new { id = Id });
                    }
                }

                if (newAdmin != null)
                {
                    group.AdminList.Add(new Admin { AdminId = newAdmin.Id, Email = newAdmin.Email, UserName = newAdmin.NickName });
                    _db.Groups.Update(group);
                    _db.SaveChanges();
                    TempData["success"] = "Success";
                    return RedirectToAction("Edit", new { id = Id });
                }
            }
            TempData["Error"] = "Add admin failed";
            return RedirectToAction("Edit", new { id = Id });
        }

    }
}
