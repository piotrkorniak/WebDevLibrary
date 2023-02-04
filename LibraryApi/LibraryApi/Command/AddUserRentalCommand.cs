using MediatR;

namespace LibraryApi.Command
{
    public class AddUserRentalCommand : IRequest
    {
        public int BookId { get; init; }
        public int? UserId { get; private set; }
    }
}