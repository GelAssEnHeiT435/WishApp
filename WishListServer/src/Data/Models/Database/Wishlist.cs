using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishListServer.src.Data.Models.Database
{
    public class Wishlist
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WishlistId { get; set; }
        public Guid ShareToken { get; set; }
        
        public Guid UserId { get; set; }
        public User User { get; set; }

        public ICollection<Wish>? Wishes { get; set; }
    }
}
