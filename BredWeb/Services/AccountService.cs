using BredWeb.Data;
using BredWeb.Interfaces;
using BredWeb.Models;
using Microsoft.AspNetCore.Identity;
namespace BredWeb.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;

        public AccountService(ApplicationDbContext db)
        {
            this._db = db;
        }

        public AccountViewModel GetAccountViewModel(Person user)
        {
            var posts = _db.Posts.Where(p => p.AuthorName == user.NickName).ToList();
            AccountViewModel model = new();
            model.Person = user;
            model.Posts = posts;
            model.Statistics = GetAccountStatistics(user);
            return model;
        }

        private Statistics GetAccountStatistics(Person user)
        {
            return new Statistics
            { 
                GroupsCreated = GetGroupsCreatedCount(user.NickName!),
                ModeratedGroups = GetModeratedGroupCount(user.Id),
                JoinedGroups = GetJoinedGroupCount(user),
                PostCount = GetPostCount(user.NickName!),
                CommentCount = GetCommentCount(user.NickName!),
                TotalRating = GetTotalRating(user.NickName!)
            };
        }

        private int GetGroupsCreatedCount(string user)
        {
            return _db.Groups.Count(g => g.Creator == user);
        }

        private int GetModeratedGroupCount(string user)
        {
            return _db.Admins.Count(a => a.AdminId == user);
        }

        private int GetJoinedGroupCount(Person user)
        {
            return _db.Groups.Count(g => g.UserList.Contains(user));
        }

        private int GetPostCount(string user)
        {
            return _db.Posts.Count(p => p.AuthorName == user);
        }

        private int GetCommentCount(string user)
        {
            return _db.Comments.Count(c => c.AuthorName == user);
        }

        private int GetTotalRating(string user)
        {
            return _db.Posts
                   .Where(p => p.AuthorName == user)
                   .Select(p => p.TotalRating)
                   .Sum();
        }
    }
}
