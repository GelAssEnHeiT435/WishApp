using System.ComponentModel.DataAnnotations;

namespace WishListServer.src.Options
{
    public class JwtOptions
    {
        [Required]
        [MaxLength(32)]
        public string SecretKey { get; set; }

        [Required] public string Issuer { get; set; }
        [Required] public string Audience { get; set; }
        [Required] public int ExpirationInMinutes { get; set; }
    }
}
