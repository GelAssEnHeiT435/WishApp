namespace WishListServer.src.Data.Models.Dto
{
    public class CreateWishDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsRecieved { get; set; }
    }
}
