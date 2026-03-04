namespace WishListServer.src.Data.Models.Dto
{
    public class WishDto
    {
        public Guid WishId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsReceived { get; set; }
        public string? Url { get; set; }
    }
}
