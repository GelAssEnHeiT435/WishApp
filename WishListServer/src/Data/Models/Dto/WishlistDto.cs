using WishListServer.src.Data.Models.Database;

namespace WishListServer.src.Data.Models.Dto
{
    public class WishlistDto
    {
        public UserDto User { get; set; }
        public ICollection<WishDto>? Wishes { get; set; }
    }
}
