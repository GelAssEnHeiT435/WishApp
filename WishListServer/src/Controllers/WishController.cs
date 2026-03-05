using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Core.Handlers;
using WishListServer.src.Data.Models.Common;
using WishListServer.src.Data.Models.Dto;

namespace WishListServer.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WishController(IMediator mediator) =>
            _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<WishDto>>> GetWishes(CancellationToken ct)
        {
            string url = $"{Request.Scheme}://{Request.Host}";

            GetWishesCommand command = new GetWishesCommand(url);
            IReadOnlyCollection<WishDto> wishes = await _mediator.Send(command, ct);

            return Ok(wishes);
        }

        [HttpGet("{wishId:guid}")]
        public async Task<ActionResult<WishDto>> GetWishById(
            Guid wishId, 
            CancellationToken ct)
        {
            string url = $"{Request.Scheme}://{Request.Host}";
            GetWishByIdCommand command = new GetWishByIdCommand(wishId, url);

            try
            {
                return Ok(await _mediator.Send(command, ct));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CreateWishResult?>> CreateWish(
            [FromForm] CreateWishDto wish, 
            IFormFile? image, 
            CancellationToken ct)
        {
            CreateWishCommand command = new CreateWishCommand(
                wish.Title, 
                wish.Description, 
                wish.IsRecieved, 
                image
            );

            CreateWishResult? result = await _mediator.Send(command, ct);

            result.Path = result.Path != null
                ? $"{Request.Scheme}://{Request.Host}{result.Path}"
                : null;

            return Ok(result);
        }

        [HttpPatch("{wishId:guid}")]
        public async Task<ActionResult<UpdateWishResult>> UpdateWish(
            Guid wishId,
            [FromForm] UpdateWishDto wish,
            IFormFile? image,
            CancellationToken ct)
        {
            UpdateWishCommand command = new UpdateWishCommand(
                wishId, wish.Title, wish.Description, wish.IsRequired, image);
            
            try
            {
                return Ok(await _mediator.Send(command, ct));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{wishId:guid}")]
        public async Task<ActionResult> DeleteWish(
            Guid wishId,
            CancellationToken ct)
        {
            DeleteWishCommand command = new DeleteWishCommand(wishId);

            try
            {
                await _mediator.Send(command, ct);
                return NoContent();
            }
            catch(EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
