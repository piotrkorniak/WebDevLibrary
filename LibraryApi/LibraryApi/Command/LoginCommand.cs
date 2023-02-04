using LibraryApi.DTO;
using MediatR;

namespace LibraryApi.Command
{
    public class LoginCommand : IRequest<AuthenticationResponse>
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }
}