namespace WishListServer.src.Data.Models.Common
{
    public class CreateWishResult(Guid WishId, string? Path)
    {
        public Guid WishId { get; set; } = WishId;
        public string? Path { get; set; } = Path;
    }
}
