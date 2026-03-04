using Microsoft.EntityFrameworkCore;
using WishListServer.src.Data.Models.Database;
using WishListServer.src.Data.Models.Dto;

namespace WishListServer.src.Data.Specifications
{
    public static class WishQueries
    {
        public static IEnumerable<WishDto> ToDto(this IQueryable<Wish> query, string baseUrl) =>
            query.Include(wish => wish.Image)
            .AsEnumerable()
            .Select(wish => new WishDto
            {
                WishId = wish.WishId,
                Title = wish.Title,
                Description = wish.Description,
                IsReceived = wish.IsReceived,
                Url = wish.Image != null && !string.IsNullOrEmpty(wish.Image.Url)
                        ? $"{baseUrl}/{wish.Image.Url.TrimStart('/')}"
                        : null,
            });
    }
}
