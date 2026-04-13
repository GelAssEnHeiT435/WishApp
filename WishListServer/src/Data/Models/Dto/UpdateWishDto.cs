namespace WishListServer.src.Data.Models.Dto
{
    public class UpdateWishDto
    {
        public string? Title {  get; set; }
        public string? Description { get; set; }
        public string? Link { get; set; }
        public bool? IsReceived { get; set; }
    }
}
