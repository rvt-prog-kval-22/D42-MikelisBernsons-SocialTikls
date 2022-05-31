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

            if (groupId is 0 || postId is 0 || body is null)
                return RedirectToAction("OpenPost", "Post", new { groupId = groupId, postId = postId });

            Person user = await _userManager.GetUserAsync(User);
            Post? post = _db.Posts.Find(postId);

            if (post == null)
                return RedirectToAction("OpenPost", "Post", new { groupId = groupId, postId = postId });

            if (body.Length >= 4)
            {
                post.CommentList!.Add(
                new Comment{
                    Body = body,
                    AuthorName = user.NickName!,
                    PostDate = DateTime.Now
                });
            
                _db.Posts.Update(post);
                _db.SaveChanges();
                TempData["success"] = "Success";
            }            
            return RedirectToAction("OpenPost", "Post", new { groupId = groupId, postId = postId});
        }

        [Authorize]
        public async Task<IActionResult> Delete(int groupId, int id, int postId)
        {
            Comment? comment = _db.Comments.Find(id);
            var group = _db.Groups.Find(groupId);

            if (comment == null || group == null)
                return RedirectToAction("OpenPost", "Post", new { groupId = groupId, postId = postId });

            _db.Entry(group).Collection(g => g.AdminList!).Load();

            var user = (await _userManager.GetUserAsync(User));
            var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if(isAdmin || group.AdminList!.Any(x => x.AdminId == user.Id) || user.NickName == comment.AuthorName)
            {
                _db.Comments.Remove(comment);
                _db.SaveChanges();
                TempData["success"] = "Success";
                return RedirectToAction("OpenPost", "Post", new { groupId = groupId, postId = postId });
            }
            return RedirectToAction("OpenPost", "Post", new { groupId = groupId, postId = postId });
        }

    }
}
