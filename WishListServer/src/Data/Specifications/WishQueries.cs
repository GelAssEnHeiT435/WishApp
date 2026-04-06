using Microsoft.EntityFrameworkCore;
using WishListServer.src.Data.Models.Database;
using WishListServer.src.Data.Models.Dto;
using WishListServer.src.Data.Specifications;

namespace WishListServer.src.Data.Specifications
{
    public static class WishQueries
    {
        public static IQueryable<WishDto> ToDto(this IQueryable<Wish> query, string baseUrl) =>
            query.Select(wish => new WishDto
            {
                WishId = wish.WishId,
                Title = wish.Title,
                Description = wish.Description,
                Link = wish.Link,
                IsReceived = wish.IsReceived,
                Url = wish.Image != null && wish.Image.Url != null
                    ? baseUrl + wish.Image.Url
                    : null
            });

        public static IQueryable<WishlistDto> ToDto(this IQueryable<Wishlist> query, string baseUrl) =>
            query.Select(wl => new WishlistDto
            {
                User = new UserDto
                {
                    UserId = wl.User.UserId,
                    Username = wl.User.Username
                },
                Wishes = wl.Wishes != null
                    ? wl.Wishes.Select(w => new WishDto
                    {
                        WishId = w.WishId,
                        Title = w.Title,
                        Description = w.Description,
                        Link = w.Link,
                        IsReceived = w.IsReceived,
                        Url = w.Image != null && w.Image.Url != null
                            ? baseUrl + w.Image.Url
                            : null
                    }).ToList()
                    : new List<WishDto>()
            });
    }
}
