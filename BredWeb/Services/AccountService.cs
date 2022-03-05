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
            return model;
        }
    }
}
