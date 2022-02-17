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

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Delete(int groupId, int id, int postId)
        {
            Comment? comment = _db.Comments.Find(id);

            if (comment == null)
                return NotFound();            

            _db.Comments.Remove(comment);
            _db.SaveChanges();
            TempData["success"] = "Success";
            return RedirectToAction("OpenPost", "Post", new { groupId = groupId, postId = postId });
        }

    }
}
