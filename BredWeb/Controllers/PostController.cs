using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace BredWeb.Controllers
{
    public class PostController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public PostController(UserManager<Person> userManager,
                                 SignInManager<Person> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 ApplicationDbContext db,
                                 IWebHostEnvironment hostingEnvironment)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._hostingEnvironment = hostingEnvironment;
            _db = db;
        }

        //GET
        public IActionResult Index()
        {
            return View();
        }

        //GET
        [HttpGet]
        [Authorize]
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
            
            if (ModelState.IsValid)
            {
                group.Posts!.Add(post);
                _db.Groups.Update(group);
                _db.SaveChanges();
                TempData["success"] = "Post created successfully";
            }            
            
            return RedirectToAction("Open", "Group", new { id = groupId});
        }

        //POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateYT(Post post, int groupId)
        {

            if (groupId == 0)
                return NotFound();

            Group? group = _db.Groups.Find(groupId);
            Person user = await _userManager.GetUserAsync(User);

            if (group == null)
                return NotFound();

            ViewBag.GroupTitle = group.Title;
            post.Type = Post.TypeEnum.Youtube;
            post.AuthorName = user.NickName;
            post.PostDate = DateTime.Now;
            post.Id = 0; // this fixes an error

            if (ModelState.IsValid)
            {
                post.Body = post.Body!.Replace("watch?v=", "embed/");
                group.Posts!.Add(post);
                _db.Groups.Update(group);
                _db.SaveChanges();
                TempData["success"] = "Post created successfully";
            }

            return RedirectToAction("Open", "Group", new { id = groupId });
        }
        //POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateImage(CreatePost post, int groupId)
        {
            Group? group = _db.Groups.Find(groupId);
            if (group == null)
                return NotFound();

            string[] allowedFileTypes = {"image/jpeg" , "image/jpg", "image/png", "image/gif" };
            bool allowedType = false;
            if (post.Image!.Length > 15000000)
            {
                return RedirectToAction("Create", new { id = groupId, groupTitle = group.Title});
            }

            foreach(string type in allowedFileTypes)
            {
                if (type.Equals(post.Image.ContentType))
                {
                    allowedType = true;
                    break;
                }
            }
            if (!allowedType)
                return RedirectToAction("Create", new { id = groupId, groupTitle = group.Title });

            Person user = await _userManager.GetUserAsync(User);
            Post model = new();
            string fileName;

            ViewBag.GroupTitle = group.Title;
            model.Type = Post.TypeEnum.Image;
            model.AuthorName = user.NickName;
            model.PostDate = DateTime.Now;
            model.Id = 0; // this fixes an error
            model.Body = "";
            model.Title = post.Title;

            if (post.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                fileName = Guid.NewGuid().ToString() + "_" + post.Image.FileName;
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await post.Image.CopyToAsync(stream);
                    stream.Close();
                }

                model.ImagePath = fileName;

                group.Posts!.Add(model);
                _db.Groups.Update(group);
                _db.SaveChanges();
                TempData["success"] = "Post created successfully";
            }

            return RedirectToAction("Open", "Group", new { id = groupId });
        }

        //GET
        public async Task<IActionResult> BrowseGroup(int id, bool popular = false, string filter = "All")
        {
            var group = _db.Groups.Find(id);

            if (group == null)
                return NotFound();

            _db.Entry(group).Collection(g => g.AdminList!).Load();

            ViewBag.Group = group;

            if (_signInManager.IsSignedIn(User))
                ViewBag.nick = (await _userManager.GetUserAsync(User)).NickName;
            else
                ViewBag.nick = "";

            List<Post> posts = _db.Posts.Where(p => p.GroupId == group.Id).ToList();

            switch (filter)
            {
                case "Day":
                    posts = posts.Where(p => p.PostDate > DateTime.Now.AddDays(-1)).ToList();
                    break;
                case "Week":
                    posts = posts.Where(p => p.PostDate > DateTime.Now.AddDays(-7)).ToList();
                    break;
                case "Month":
                    posts = posts.Where(p => p.PostDate > DateTime.Now.AddDays(-30)).ToList();
                    break;
                case "Year":
                    posts = posts.Where(p => p.PostDate > DateTime.Now.AddDays(-365)).ToList();
                    break;
            }

            if (popular)
                posts = posts.OrderBy(p => p.TotalRating).ToList();

            ViewBag.Filter = filter;
            ViewBag.Popular = popular;

            return View(posts);
        }

        //GET
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var post = _db.Posts.Find(id);

            if (post == null)
                return NotFound();

            if(post.Type is Post.TypeEnum.Youtube or Post.TypeEnum.Image)
                return Unauthorized();

            var user = (await _userManager.GetUserAsync(User));
            if (post.AuthorName == user.NickName)
                return View(post);
            else
                return Unauthorized();
        }

        //POST
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Post obj)
        {
            Post? post = _db.Posts.Find(obj.Id);
            if (post == null)
                return NotFound();

            var user = (await _userManager.GetUserAsync(User));
            if (post.AuthorName == user.NickName)
            {
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
            return Unauthorized();            
        }

        //GET
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            Post? post = _db.Posts.Find(id);

            if (post == null)
                return NotFound();

            var user = (await _userManager.GetUserAsync(User));
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (user.NickName == post.AuthorName || isAdmin)
                return View(post);
            return Unauthorized();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(Post obj)
        {
            Post? post = _db.Posts.Find(obj.Id);

            if (post == null)
                return NotFound();

            var user = (await _userManager.GetUserAsync(User));
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");
            if (user.NickName == post.AuthorName || isAdmin)
            {
                if (post.Type is Post.TypeEnum.Image)
                    DeleteFile(post);

                _db.Ratings.RemoveRange(_db.Ratings.Where(r => r.RatedItemId == post.Id));
                _db.Posts.Remove(post);
                _db.SaveChanges();
                TempData["success"] = "Success";
                return RedirectToAction("BrowseGroup", "Post", new { id = post.GroupId });
            }
            return Unauthorized();
        }

        private void DeleteFile(Post post)
        {
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
            string filePath = Path.Combine(uploadsFolder, post.ImagePath!);

            if (System.IO.File.Exists(filePath))
            {
                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch { }
            }
        }

        //GET
        public async Task<IActionResult> OpenPost(int postId, int groupId)
        {
            Group? group = _db.Groups.Find(groupId);
            Post? post = _db.Posts.Find(postId);

            if (group == null || post == null)
                return NotFound();

            OpenPost model = new();

            if (_signInManager.IsSignedIn(User))
            {
                model.UserNick = (await _userManager.GetUserAsync(User)).NickName!;
                model.UserId = (await _userManager.GetUserAsync(User)).Id;
            }

            _db.Entry(group).Collection(g => g.AdminList!).Load();
            model.Group = group;
            model.Post = post;
            model.Comments = _db.Comments.Where(c => c.PostId == postId).ToList();

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Upvote(int postId,
                                                int groupId = 0,
                                                bool selfRedirect = false,
                                                bool home = false,
                                                bool account = false,
                                                string filter = "All",
                                                bool popular = false)
        {
            var post = _db.Posts.Find(postId);
            if (post == null)
                return BadRequest();
            string userId = "";
            if(_signInManager.IsSignedIn(User))
                userId = (await _userManager.GetUserAsync(User)).Id.ToString();
            else
                return RedirectToAction("Index", "Group");

            var rating = _db.Ratings.FirstOrDefault( r =>
                r.RatedItemId.Equals(postId) &&
                r.UserId!.Equals(userId)
                );

            if(rating is not null)
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
                post.RatingList!.Add(new Rating { 
                    UserId = (await _userManager.GetUserAsync(User)).Id.ToString(),
                    RatedItemId = postId,
                    Value = Rating.Status.Upvoted
                });
                post.TotalRating++;
            }

            _db.Posts.Update(post);
            _db.SaveChanges();

            string url = "";

            if(selfRedirect)
                return RedirectToAction("OpenPost", new { groupId = groupId, postId = post.Id});
            if (home)
            {
                url = Url.Action("Index", "Home");
                url += $"#{post.Id}";
                return Redirect(url);
            }
            if (account)
            {
                url = Url.Action("Index", "Account");
                url += $"#{post.Id}";
                return Redirect(url);
            }

            url = Url.Action("BrowseGroup", new {id = post.GroupId, popular = popular, filter = filter});
            url += $"#{post.Id}";
            return Redirect(url);
        }

        [Authorize]
        public async Task<IActionResult> Downvote(int postId,
                                                  int groupId = 0,
                                                  bool selfRedirect = false,
                                                  bool home = false,
                                                  bool account = false,
                                                  string filter = "All",
                                                  bool popular = false)
        {
            var post = _db.Posts.Find(postId);
            if (post == null)
                return BadRequest();
            string userId = "";
            if (_signInManager.IsSignedIn(User))
                userId = (await _userManager.GetUserAsync(User)).Id.ToString();
            else
                return RedirectToAction("Index", "Group");

            var rating = _db.Ratings.FirstOrDefault(r =>
               r.RatedItemId.Equals(postId) &&
               r.UserId!.Equals(userId)
                );

            if (rating is not null)
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
                post.RatingList!.Add(new Rating
                {
                    UserId = (await _userManager.GetUserAsync(User)).Id.ToString(),
                    RatedItemId = postId,
                    Value = Rating.Status.Downvoted
                });
                post.TotalRating--;
            }

            _db.Posts.Update(post);
            _db.SaveChanges();

            string url = "";

            if (selfRedirect)
                return RedirectToAction("OpenPost", new { groupId = groupId, postId = post.Id });
            if (home)
            {
                url = Url.Action("Index", "Home");
                url += $"#{post.Id}";
                return Redirect(url);
            }
            if (account)
            {
                url = Url.Action("Index", "Account");
                url += $"#{post.Id}";
                return Redirect(url);
            }

            url = Url.Action("BrowseGroup", new { id = post.GroupId, popular = popular, filter = filter });
            url += $"#{post.Id}";
            return Redirect(url);
        }

    }
}
