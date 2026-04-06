using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WishListServer.src.Data.Models.Database
{
    public class Credential
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CredId { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
