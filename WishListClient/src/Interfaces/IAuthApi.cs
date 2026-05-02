using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WishListClient.src.Models;

namespace WishListClient.src.Interfaces
{
    public interface IAuthApi
    {
        [Post("/api/auth/register")]
        Task<AuthResponse> Register([Body] RegisterRequest request);

        [Post("/api/auth/login")]
        Task<AuthResponse> Login([Body] LoginRequest request);
    }
}
