using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// 
        /// </summary>
        // Возвращаем IApiResponse, чтобы иметь возможность проверить статус 404 без исключения, 
        // либо можно ловить ApiException снаружи.
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
            StreamPart? image,
            CancellationToken ct = default
        );

        // PATCH /api/wish/{wishId}
        //[Multipart]
        //[Patch("/api/wish/{wishId}")]
        //Task<UpdateWishResult> UpdateWish(
        //    Guid wishId,
        //    [AliasAs("Title")] string? title,
        //    [AliasAs("Description")] string? description,
        //    [AliasAs("IsRequired")] bool isRequired,
        //    StreamPart? image, // Файл опционален. Если null, Refit может отправить пустую часть, проверьте поведение сервера.
        //    CancellationToken ct = default
        //);

        // DELETE /api/wish/{wishId}
        // Возвращает Task, так как сервер возвращает 204 NoContent.
        // При 404 или 400 будет выброшено ApiException.
        //[Delete("/api/wish/{wishId}")]
        //Task DeleteWish(Guid wishId, CancellationToken ct = default);
    }
}
