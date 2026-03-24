using Refit;
using WishListClient.src.Models;

namespace WishListClient.src.Services
{
    public interface IWishListApi
    {
        /// <summary>
        /// GET /api/wish
        /// Get all entities from server's database
        /// </summary>
        [Get("/api/wish")]
        Task<IReadOnlyCollection<Wish>> GetWishes(CancellationToken ct = default);

        /// <summary>
        /// GET /api/wish/{wishId}
        /// Get wish by Id
        /// </summary>
        [Get("/api/wish/{wishId}")]
        Task<Wish> GetWishById(Guid wishId, CancellationToken ct = default);

        // POST /api/wish
        // Важно: [Multipart] обязателен для отправки форм с файлами
        // Имена параметров (title, description и т.д.) должны совпадать с тем, как сервер ожидает их в форме,
        // либо используйте [AliasAs], если имена в DTO отличаются от ожидаемых в форме.
        // В вашем контроллере: [FromForm] CreateWishDto wish -> Refit распакует свойства DTO в поля формы автоматически,
        // НО для файла нужен отдельный параметр.
        [Multipart]
        [Post("/api/wish")]
        Task<CreateWishResponse> CreateWish(
            [AliasAs("Title")] string title,
            [AliasAs("Description")] string? description,
            [AliasAs("IsRecieved")] bool isRecieved,
            [AliasAs("Image")] ByteArrayPart? image,
            CancellationToken ct = default
        );

        /// <summary>
        /// PATCH /api/wish/{wishId}
        /// Update wish 
        /// </summary>
        [Multipart]
        [Patch("/api/wish/{wishId}")]
        Task<UpdateWishResponse> UpdateWish(
            Guid wishId,
            [AliasAs("Title")] string title,
            [AliasAs("Description")] string? description,
            [AliasAs("IsRecieved")] bool isRecieved,
            ByteArrayPart? image,
            CancellationToken ct = default
        );

        /// <summary>
        /// DELETE /api/wish/{wishId}
        /// </summary>
        [Delete("/api/wish/{wishId}")]
        Task DeleteWish(Guid wishId, CancellationToken ct = default);
    }
}
