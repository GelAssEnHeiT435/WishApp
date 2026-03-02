using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishListClient.src.Models
{
    public record class CreateWishResponse(Guid WishId, string? Path);
}
