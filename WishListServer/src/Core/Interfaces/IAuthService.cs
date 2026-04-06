using WishListServer.src.Data.Models.Database;

namespace WishListServer.src.Core.Interfaces
{
    public interface IAuthService
    {
        string GenerateJwt(User user);
    }
}
