using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishListServer.src.Data.Models.Database
{
    public class Image
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ImageId { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }

        public Guid WishId { get; set; }
        public Wish? Wish { get; set; }
    }
}