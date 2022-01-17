﻿using BredWeb.Data;
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
            var user = await userManager.GetUserAsync(User);

            if (ModelState.IsValid)
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
            var user = await userManager.GetUserAsync(User);

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

            return View("Browse", group);
            //return RedirectToAction("Open", id); //goes to this controllers "index", to go to a different controller use ("action", "controllerName")
        }

        //GET
        [Authorize]
        public IActionResult Open(int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var groupFromDb = _db.Groups.Find(id);

            if (groupFromDb == null)
                return NotFound();

            return View("Browse", groupFromDb);
        }
    }
}