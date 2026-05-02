using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishListClient.src.Interfaces
{
    public interface IAuthService
    {
        Task UserRegister(string login, string username, string password);
        Task UserLogin(string login, string password);
    }
}
