using Refit;
using WishListClient.src.Models;

namespace WishListClient.src.Interfaces
{
    public interface IWishListApi
    {
        [Get("/api/wish")]
        Task<IReadOnlyCollection<Wish>> GetWishes(CancellationToken ct = default);


        [Get("/api/wish/{wishId}")]
        Task<Wish> GetWishById(Guid wishId, CancellationToken ct = default);


        [Multipart]
        [Post("/api/wish")]
        Task<CreateWishResponse> CreateWish(
            [AliasAs("Title")] string title,
            [AliasAs("Description")] string? description,
            [AliasAs("Link")] string? link,
            [AliasAs("IsReceived")] bool isReceived,
            [AliasAs("Image")] ByteArrayPart? image,
            CancellationToken ct = default
        );


        [Multipart]
        [Patch("/api/wish/{wishId}")]
        Task<UpdateWishResponse> UpdateWish(
            Guid wishId,
            [AliasAs("Title")] string title,
            [AliasAs("Description")] string? description,
            [AliasAs("Link")] string? link,
            [AliasAs("IsReceived")] bool isReceived,
            ByteArrayPart? image,
            CancellationToken ct = default
        );


        [Delete("/api/wish/{wishId}")]
        Task DeleteWish(Guid wishId, CancellationToken ct = default);


        [Get("/api/wish/share-link")]
        Task<ShareResponse> GetShareLink(CancellationToken ct = default);


        [Post("/api/wish/regenerate-link")]
        Task<ShareResponse> RegenerateShareLink(CancellationToken ct = default);
    }
}
