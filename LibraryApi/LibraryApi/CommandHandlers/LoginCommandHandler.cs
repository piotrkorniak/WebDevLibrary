using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Command;
using LibraryApi.Context;
using LibraryApi.DTO;
using LibraryApi.Exceptions;
using LibraryApi.Infrastructure;
using LibraryApi.Services.Abstracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.CommandHandlers
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
    {
        private readonly LibraryContext _context;
        private readonly ITokenProvider _tokenProvider;

        public LoginCommandHandler(ITokenProvider tokenProvider, LibraryContext context)
        {
            _tokenProvider = tokenProvider;
            _context = context;
        }

        public async Task<AuthenticationResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var emailInLowercase = request.Email.ToLower();
            var user = await _context.Users.Where(x => x.Email == emailInLowercase)
                .FirstOrDefaultAsync(cancellationToken);

            Contract.Requires(user != null && user.IsPasswordMatch(request.Password),
                ErrorCode.LoginErrors.InvalidCredentials);

            var tokenData = _tokenProvider.Create(user.Id, user.Role);

            var loginResponse = new AuthenticationResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                Token = tokenData.Token,
                TokenExpirationDate = tokenData.ExpirationTimestamp
            };
            return loginResponse;
        }
    }
}