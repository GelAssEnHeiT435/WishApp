using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Core.Handlers.WishlistHandlers;
using WishListServer.src.Data.Models.Common;
using WishListServer.src.Data.Models.Dto;

namespace WishListServer.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WishController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WishController> _logger;

        public WishController(IMediator mediator, ILogger<WishController> logger) 
        { 
            _mediator = mediator; 
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<WishDto>>> GetWishes(CancellationToken ct)
        {
            string url = $"{Request.Scheme}://{Request.Host}";
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            GetWishesCommand command = new GetWishesCommand(userId, url);

            try {
                return Ok(await _mediator.Send(command, ct));
            }
            catch (EntityNotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{wishId:guid}")]
        public async Task<ActionResult<WishDto>> GetWishById(
            Guid wishId, 
            CancellationToken ct)
        {
            string url = $"{Request.Scheme}://{Request.Host}";
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            GetWishByIdCommand command = new GetWishByIdCommand(userId, wishId, url);

            try {
                return Ok(await _mediator.Send(command, ct));
            }
            catch (EntityNotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("share-link")]
        public async Task<ActionResult<WishlistLinkDto>> GetShareLink(CancellationToken ct)
        {
            string url = $"{Request.Scheme}://{Request.Host}";
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            GetShareLinkCommand command = new GetShareLinkCommand(userId, url);

            try {
                return Ok(await _mediator.Send(command, ct));
            }
            catch (EntityNotFoundException) {
                return NotFound("The user has no wishlists");
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("regenerate-link")]
        public async Task<ActionResult<WishlistLinkDto>> RegenerateLink(CancellationToken ct)
        {
            string url = $"{Request.Scheme}://{Request.Host}";
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            RegenerateLinkCommand command = new RegenerateLinkCommand(userId, url);

            try
            {
                return Ok(await _mediator.Send(command, ct));
            }
            catch (EntityNotFoundException)
            {
                return NotFound("The user has no wishlists");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CreateWishResult?>> CreateWish(
            [FromForm] CreateWishDto wish, 
            CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            CreateWishCommand command = new CreateWishCommand(
                userId,
                wish.Title, 
                wish.Description, 
                wish.Link,
                wish.IsRecieved, 
                wish.Image
            );

            try {
                CreateWishResult? result = await _mediator.Send(command, ct);

                result.Path = result.Path != null
                    ? $"{Request.Scheme}://{Request.Host}{result.Path}"
                    : null;

                return Ok(result);
            }
            catch (EntityNotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{wishId:guid}")]
        public async Task<ActionResult<UpdateWishResult>> UpdateWish(
            Guid wishId,
            [FromForm] UpdateWishDto wish,
            IFormFile? image,
            CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            UpdateWishCommand command = new UpdateWishCommand(
                userId, wishId, wish.Title, wish.Description, wish.Link, wish.IsRecieved, image
            );
            
            try {
                UpdateWishResult result = await _mediator.Send(command, ct);

                string? Path = result.Path != null
                    ? $"{Request.Scheme}://{Request.Host}{result.Path}"
                    : null;
                result = result with { Path = Path };

                return Ok(result);
            }
            catch (EntityNotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{wishId:guid}")]
        public async Task<ActionResult> DeleteWish(
            Guid wishId,
            CancellationToken ct)
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            DeleteWishCommand command = new DeleteWishCommand(userId, wishId);

            try {
                await _mediator.Send(command, ct);
                return NoContent();
            }
            catch(EntityNotFoundException ex) {
                return NotFound(ex.Message);
            }
            catch(Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}
