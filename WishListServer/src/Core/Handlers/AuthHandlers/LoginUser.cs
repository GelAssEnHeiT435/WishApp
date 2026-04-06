using MediatR;
using Microsoft.EntityFrameworkCore;
using WishListServer.src.Core.Exceptions;
using WishListServer.src.Core.Interfaces;
using WishListServer.src.Data;
using WishListServer.src.Data.Models.Common;
using WishListServer.src.Data.Models.Database;

namespace WishListServer.src.Core.Handlers.AuthHandlers
{
    public record class LoginUserCommand(string login, string password): IRequest<JwtResult>;
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, JwtResult>
    {
        private readonly ApplicationContext _context;
        private readonly IAuthService _auth;

        public LoginUserHandler(ApplicationContext context, IAuthService auth)
        {
            _context = context;
            _auth = auth;
        }

        public async Task<JwtResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            User? user = await _context.Users
                .Include(u => u.Credential)
                .FirstOrDefaultAsync(u => request.login == u.Credential.Login);

            if (user == null)
                throw new EntityNotFoundException();

            if (!BCrypt.Net.BCrypt.Verify(request.password, user.Credential.PasswordHash))
                throw new NotMatchPasswordException("The entered password is incorrect");

            return new JwtResult(_auth.GenerateJwt(user));
        }
    }
}
