using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BredWeb.Controllers
{
    public class PostController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PostController(UserManager<Person> userManager,
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
            Person user = await _userManager.GetUserAsync(User);

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
        public async Task<IActionResult> BrowseGroup(int id)
        {
            var group = _db.Groups.Find(id);

            if (group == null)
                return NotFound();

            _db.Entry(group).Collection(g => g.AdminList).Load();

            ViewBag.Group = group;

            if (_signInManager.IsSignedIn(User))
                ViewBag.nick = (await _userManager.GetUserAsync(User)).NickName;
            else
                ViewBag.nick = "";

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
            post.IsEdited = true;

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

            _db.Ratings.RemoveRange(_db.Ratings.Where(r => r.RatedItemId == post.Id));
            _db.Posts.Remove(post);
            _db.SaveChanges();
            TempData["success"] = "Success";
            return RedirectToAction("BrowseGroup", "Post", new { id = post.GroupId });
        }

        //GET
        public async Task<IActionResult> OpenPost(int postId, int groupId)
        {
            Group? group = _db.Groups.Find(groupId);
            Post? post = _db.Posts.Find(postId);

            if (group == null || post == null)
                return NotFound();

            if (_signInManager.IsSignedIn(User))
                ViewBag.nick = (await _userManager.GetUserAsync(User)).NickName;
            else
                ViewBag.nick = "";

            ViewBag.Post = post;

            ViewBag.GroupId = group.Id;
            ViewBag.GroupTitle = group.Title;
            ViewBag.UserCount = group.UserCount;
            ViewBag.Title = group.Title;
            ViewBag.Creator = group.Creator;
            ViewBag.Description = group.Description;
            //TODO: Remake to an object... since you can do that... :[
            List<Comment> comments = _db.Comments.Where(c => c.PostId == postId).ToList();

            return View(comments);
        }

        public async Task<IActionResult> Upvote(int postId, bool selfRedirect = false, int groupId = 0)
        {
            var post = _db.Posts.Find(postId);
            string userId = "";
            if(_signInManager.IsSignedIn(User))
                userId = (await _userManager.GetUserAsync(User)).Id.ToString();
            else
                return RedirectToAction("Index", "Group");

            var rating = _db.Ratings.FirstOrDefault( r =>
                r.RatedItemId.Equals(postId) &&
                r.UserId.Equals(userId)
                );

            if(rating != null)
            {
                if (rating.Value == Rating.Status.Upvoted)
                {
                    rating.Value = Rating.Status.Nothing;
                    post.TotalRating--;
                }
                else if(rating.Value == Rating.Status.Nothing)
                {
                    rating.Value = Rating.Status.Upvoted;
                    post.TotalRating++;
                }
                else
                {
                    rating.Value = Rating.Status.Upvoted;
                    post.TotalRating += 2;
                }

            }
            else
            {
                post.RatingList.Add(new Rating { 
                    UserId = (await _userManager.GetUserAsync(User)).Id.ToString(),
                    RatedItemId = postId,
                    Value = Rating.Status.Upvoted
                });
                post.TotalRating++;
            }

            _db.Posts.Update(post);
            _db.SaveChanges();
            if(selfRedirect)
                return RedirectToAction("OpenPost", new { groupId = groupId, postId = post.Id});
            return RedirectToAction("BrowseGroup", new { id = post.GroupId });
        }

        public async Task<IActionResult> Downvote(int postId, bool selfRedirect = false, int groupId = 0)
        {
            var post = _db.Posts.Find(postId);
            string userId = "";
            if (_signInManager.IsSignedIn(User))
                userId = (await _userManager.GetUserAsync(User)).Id.ToString();
            else
                return RedirectToAction("Index", "Group");

            var rating = _db.Ratings.FirstOrDefault(r =>
               r.RatedItemId.Equals(postId) &&
               r.UserId.Equals(userId)
                );

            if (rating != null)
            {
                if (rating.Value == Rating.Status.Upvoted)
                {
                    rating.Value = Rating.Status.Downvoted;
                    post.TotalRating -= 2;
                }
                else if(rating.Value == Rating.Status.Downvoted)
                {
                    rating.Value = Rating.Status.Nothing;
                    post.TotalRating++;
                }
                else
                {
                    rating.Value = Rating.Status.Downvoted;
                    post.TotalRating--;
                }

            }
            else
            {
                post.RatingList.Add(new Rating
                {
                    UserId = (await _userManager.GetUserAsync(User)).Id.ToString(),
                    RatedItemId = postId,
                    Value = Rating.Status.Downvoted
                });
                post.TotalRating--;
            }

            _db.Posts.Update(post);
            _db.SaveChanges();
            if (selfRedirect)
                return RedirectToAction("OpenPost", new { groupId = groupId, postId = post.Id });
            return RedirectToAction("BrowseGroup", new { id = post.GroupId });
        }

    }
}
