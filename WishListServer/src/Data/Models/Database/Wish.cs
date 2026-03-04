using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishListServer.src.Data.Models.Database
{
    public class Wish
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid WishId { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public bool IsReceived { get; set; }

        public Image? Image { get; set; }
    }
}
