using BredWeb.Data;
using BredWeb.Interfaces;
using BredWeb.Models;
using Microsoft.AspNetCore.Identity;
namespace BredWeb.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _db;

        public AccountService(UserManager<Person> userManager,
                                 SignInManager<Person> signInManager,
                                 IConfiguration configuration,
                                 ApplicationDbContext db)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._configuration = configuration;
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
