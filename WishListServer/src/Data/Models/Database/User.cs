using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishListServer.src.Data.Models.Database
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }
        public string Username { get; set; }

        public Credential Credential { get; set; }
        public Wishlist Wishlist { get; set; }
    }
}
