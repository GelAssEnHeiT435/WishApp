using MediatR;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Dto;
using WishListServer.src.Data.Specifications;

namespace WishListServer.src.Core.Handlers
{
    public record class GetWishByIdCommand(Guid wishId, string baseUrl): IRequest<WishDto>;

    public class GetWishByIdHandler : IRequestHandler<GetWishByIdCommand, WishDto>
    {
        private readonly ApplicationContext _context;

        public GetWishByIdHandler(ApplicationContext context) =>
            _context = context;

        public async Task<WishDto> Handle(GetWishByIdCommand request, CancellationToken cancellationToken)
        {
            WishDto? wish = _context.Wishes
                .Where(w => w.WishId == request.wishId)
                .ToDto(request.baseUrl)
                .FirstOrDefault();

            if (wish == null) throw new EntityNotFoundException(nameof(wish), request.wishId);
            return wish;
        }
    }
}
