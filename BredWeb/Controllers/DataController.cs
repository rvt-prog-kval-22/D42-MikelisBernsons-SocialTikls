using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections;
using System.Diagnostics;

namespace BredWeb.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DataController : Controller
    {
        private readonly UserManager<Person> _userManager;
        private readonly SignInManager<Person> _signInManager;
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public DataController(UserManager<Person> userManager,
                              SignInManager<Person> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              ApplicationDbContext db,
                              IConfiguration configuration)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
            _db = db;
        }

        //GET
        public IActionResult Index()
        {
            return View();
        }

        //GET
        public IActionResult CreateAll()
        {
            return View();
        }

        public async Task<IActionResult> CreateUsers(int amount)
        {
            List<Person> people = new(amount);
            Bogus.Faker faker = new Bogus.Faker();
            Random r = new Random();

            for (int i = 0; i < amount; i++)
            {
                string word = faker.Random.Word();
                var person = new Person
                {
                    NickName = word,
                    Email = $"{word}@{word}.{word}",
                    UserName = $"{word}@{word}.{word}",
                    BirthDay = new DateTime(r.Next(1950, 2016), r.Next(1, 12), r.Next(1, 30)),
                    DateCreated = DateTime.Now
                };

                var result = await _userManager.CreateAsync(person, word);
                people.Add(person);
            }

            return View("Users", people);
        }

        public async Task<IActionResult> CreateGroups(int amount)
        {
            Bogus.Faker faker = new();
            Random r = new();

            for (int i = 0; i < amount; i++)
            {
                string title = faker.Random.Word();
                Person person = _db.People.Skip(r.Next(0, _db.People.Count())).Take(1).FirstOrDefault();

                if (_db.Groups.Any(g => g.Title == title))
                {
                    continue;
                }

                var group = new Group
                {
                    StartDate = new DateTime(r.Next(2014, 2022), r.Next(2, 10), r.Next(2, 29), 14, 16, 18, 420),
                    Creator = person.NickName,
                    Title = title,
                    Description = faker.Random.Words(r.Next(10, 100))
                };
                group.UserCount++;
                group.UserList.Add(person);
                group.AdminList.Add(new Admin { AdminId = person.Id, Email = person.Email, UserName = person.NickName! });

                _db.Groups.Add(group);
                _db.SaveChanges();
            }

            return View("CreateAll");
        }

        public IActionResult CreatePosts()
        {
            Bogus.Faker faker = new();
            Random r = new();
            var groups = _db.Groups.ToList();

            foreach (var group in groups)
            {
                _db.Entry(group).Collection(g => g.Posts!).Load();
                for (int i = 0; i < r.Next(2, 50); i++)
                {
                    var post = new Post
                    {
                        Title = faker.Random.Word(),
                        Body = faker.Random.Words(r.Next(3, 20)),
                        PostDate = new DateTime(r.Next(group.StartDate.Year, 2022), r.Next(1, 11), r.Next(1, 29), 14, 16, 18, 420),
                        AuthorName = _db.People.Skip(r.Next(0, _db.People.Count())).Take(1).FirstOrDefault().NickName,
                        GroupId = group.Id
                    };
                    group.Posts!.Add(post);
                }

                _db.Groups.Update(group);
            }
            _db.SaveChanges();

            return View("CreateAll");
        }
    }
}