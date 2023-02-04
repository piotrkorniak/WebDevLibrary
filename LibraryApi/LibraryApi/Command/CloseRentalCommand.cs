using MediatR;

namespace LibraryApi.Command
{
    public class CloseRentalCommand : IRequest
    {
        public int RentalId { get; init; }
    }
}