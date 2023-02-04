using LibraryApi.DTO;
using MediatR;

namespace LibraryApi.Command
{
    public class RegisterCommand : IRequest<AuthenticationResponse>
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }
}