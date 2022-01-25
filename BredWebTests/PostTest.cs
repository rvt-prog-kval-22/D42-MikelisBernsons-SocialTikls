using BredWeb.Controllers;
using BredWeb.Data;
using BredWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BredWebTests
{
    [TestClass]
    public class PostTest
    {
        private static readonly UserManager<Person> userManager;
        private static readonly SignInManager<Person> signInManager;
        private static readonly ApplicationDbContext _db;

        private PostController controller = new PostController(
            userManager, signInManager, _db);

        [TestMethod]
        public void TestEmptyIndex()
        {
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}