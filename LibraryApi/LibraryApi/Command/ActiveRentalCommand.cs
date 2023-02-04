using MediatR;

namespace LibraryApi.Command
{
    public class ActiveRentalCommand : IRequest
    {
        public int RentalId { get; init; }
    }
}