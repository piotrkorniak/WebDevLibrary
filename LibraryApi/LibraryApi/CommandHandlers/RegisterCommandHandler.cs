using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Command;
using LibraryApi.Context;
using LibraryApi.Domain;
using LibraryApi.DTO;
using LibraryApi.Enums;
using LibraryApi.Exceptions;
using LibraryApi.Infrastructure;
using LibraryApi.Services.Abstracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.CommandHandlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResponse>
    {
        private readonly LibraryContext _context;
        private readonly ITokenProvider _tokenProvider;

        public RegisterCommandHandler(ITokenProvider tokenProvider, LibraryContext context)
        {
            _tokenProvider = tokenProvider;
            _context = context;
        }

        public async Task<AuthenticationResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var emailInLowercase = request.Email.ToLower();
            var isEmailInUse =
                await _context.Users.AnyAsync(x => x.Email == emailInLowercase, cancellationToken);

            Contract.Requires(!isEmailInUse,
                ErrorCode.RegisterErrors.EmailInUse);

            var user = new User(request.FirstName, request.LastName, UserRole.User, emailInLowercase, request.Password);

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var tokenData = _tokenProvider.Create(user.Id, user.Role);

            var registerResponse = new AuthenticationResponse
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role.ToString(),
                Token = tokenData.Token,
                TokenExpirationDate = tokenData.ExpirationTimestamp
            };
            return registerResponse;
        }
    }
}