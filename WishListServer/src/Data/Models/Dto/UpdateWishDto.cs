namespace WishListServer.src.Data.Models.Dto
{
    public class UpdateWishDto
    {
        public string? Title {  get; set; }
        public string? Description { get; set; }
        public bool? IsRequired { get; set; }
    }
}
