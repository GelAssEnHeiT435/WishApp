using MediatR;
using Microsoft.EntityFrameworkCore;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Database;
using WishListServer.src.Data.Models.Dto;
using WishListServer.src.Data.Specifications;

namespace WishListServer.src.Core.Handlers.WishlistHandlers
{
    public record class GetWishesCommand(Guid userId, string baseUrl): IRequest<IReadOnlyCollection<WishDto>>;
    public class GetWishesHandler : IRequestHandler<GetWishesCommand, IReadOnlyCollection<WishDto>>
    {
        private readonly ApplicationContext _context;

        public GetWishesHandler(ApplicationContext context) =>
            _context = context;

        public async Task<IReadOnlyCollection<WishDto>> Handle(GetWishesCommand request, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<WishDto>? list = _context.Wishes
                .AsNoTrackingWithIdentityResolution()
                .Include(w => w.Wishlist)
                .Include(w => w.Image)
                .Where(w => w.Wishlist.UserId == request.userId)
                .ToDto(request.baseUrl)
                .ToList();

            if (list == null) throw new EntityNotFoundException();
            return list;
        }
    }
}
