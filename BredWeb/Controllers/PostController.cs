﻿using BredWeb.Data;
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

        //GET
        [HttpGet]
        public IActionResult Create(string id, string groupTitle)
        {
            ViewBag.id = id;
            ViewBag.GroupTitle = groupTitle;
            return View();
        }

        //POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Post post, int groupId)
        {

            if (groupId == 0)
                return NotFound();

            Group? group = _db.Groups.Find(groupId);
            Person user = await userManager.GetUserAsync(User);

            if (group == null)
                return NotFound();

            ViewBag.GroupTitle = group.Title;
            post.AuthorName = user.NickName;
            post.PostDate = DateTime.Now;
            post.Id = 0; // this fixes an error

            var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });
            
            if (ModelState.IsValid)
            {
                group.Posts.Add(post);
                // post counter in group?
                _db.Groups.Update(group);
                //_db.Attach(group);
                _db.SaveChanges();
                TempData["success"] = "Post created successfully";
            }            
            
            return RedirectToAction("Open", "Group", new { id = groupId});
        }

        //GET
        public IActionResult BrowseGroup(int id)
        {
            var group = _db.Groups.Find(id);

            if (group == null)
                return NotFound();

            ViewBag.GroupId = group.Id;
            ViewBag.GroupTitle = group.Title;
            //ViewBag.Description = group.Description;
            ViewBag.UserCount = group.UserCount;
            ViewBag.Title = group.Title;
            ViewBag.Creator = group.Creator;
            ViewBag.Description = group.Description;

            List<Post> posts = _db.Posts.Where(p => p.GroupId == group.Id).ToList();

            return View(posts);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var post = _db.Posts.Find(id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post obj)
        {
            Post post = _db.Posts.Find(obj.Id);
            post.Body = obj.Body;

            if (ModelState.IsValid)
            {
                _db.Posts.Update(post);
                _db.SaveChanges();
                TempData["success"] = "Success";
                return RedirectToAction("BrowseGroup", "Post", new { id = post.GroupId });
            }
            return View(obj);
        }

        //GET
        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            Post? post = _db.Posts.Find(id);

            if (post == null)
                return NotFound();

            return View(post);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Delete(Post obj)
        {
            Post? post = _db.Posts.Find(obj.Id);

            if (post == null)
                return NotFound();            

            _db.Posts.Remove(post);
            _db.SaveChanges();
            TempData["success"] = "Success";
            return RedirectToAction("BrowseGroup", "Post", new { id = post.GroupId });
        }

    }
}