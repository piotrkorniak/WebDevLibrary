using MediatR;

namespace LibraryApi.Command
{
    public class CloseUserRentalCommand : IRequest
    {
        public int RentalId { get; init; }
        public int UserId { get; init; }
    }
}