using LibraryApi.DTO;
using MediatR;

namespace LibraryApi.Query
{
    public class GetUserRentalQuery : IRequest<RentalResponse>
    {
        public int UserId { get; init; }
        public int RentalId { get; init; }
    }
}