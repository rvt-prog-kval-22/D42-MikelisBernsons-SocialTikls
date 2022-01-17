using BredWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace BredWeb.Services
{
    public class MyUserManager : UserManager<Person>
    {
        public MyUserManager(
            IUserStore<Person> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<Person> passwordHasher,
            IEnumerable<IUserValidator<Person>> userValidators,
            IEnumerable<IPasswordValidator<Person>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<Person>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {

        }



    }
}
