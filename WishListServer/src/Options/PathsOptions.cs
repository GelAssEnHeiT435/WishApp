using System.ComponentModel.DataAnnotations;

namespace WishListServer.src.Options
{
    public class PathsOptions
    {
        [Required] public string ImageStorage {  get; set; }
    }
}
