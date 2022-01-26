using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BredWeb.Controllers
{
    public class CommentController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CommentController(UserManager<Person> userManager,
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
            return NotFound();
        }

        //POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string body, int groupId, int postId)
        {

            if (groupId == 0 || postId == 0)
                return NotFound();

            Person user = await _userManager.GetUserAsync(User);
            Post? post = _db.Posts.Find(postId);

            if (post == null)
                return NotFound();

            if (body.Length >= 4)
            {
                post.CommentList.Add(
                new Comment{
                    Body = body,
                    AuthorName = user.NickName,
                    PostDate = DateTime.Now
                });
            
                _db.Posts.Update(post);
                _db.SaveChanges();
                TempData["success"] = "Success";
            }            
            return RedirectToAction("OpenPost", "Post", new { groupId = groupId, postId = postId});
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
        [Authorize]
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post obj)
        {
            Post? post = _db.Posts.Find(obj.Id);
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

        //GET
        public IActionResult OpenPost(int postId, int groupId)
        {
            Group? group = _db.Groups.Find(groupId);
            Post? post = _db.Posts.Find(postId);

            if (group == null || post == null)
                return NotFound();

            ViewBag.GroupId = group.Id;
            ViewBag.GroupTitle = group.Title;
            ViewBag.UserCount = group.UserCount;
            ViewBag.Title = group.Title;
            ViewBag.Creator = group.Creator;
            ViewBag.Description = group.Description;

            ViewBag.PostAuthor = post.AuthorName;
            ViewBag.PostBody = post.Body;
            ViewBag.PostDate = post.PostDate;
            ViewBag.PostTitle = post.Title;
            ViewBag.PostEdited = post.IsEdited;
            ViewBag.PostId = post.Id;
            //List<Comment> comments = _db.Comments.Where(c => c. == group.Id).ToList();

            return View();
        }

    }
}
