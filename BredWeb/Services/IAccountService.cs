using BredWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BredWeb.Interfaces
{
    public interface IAccountService
    {
        AccountViewModel GetAccountViewModel(Person userId);
    }
}
