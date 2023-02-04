using LibraryApi.DTO;
using MediatR;

namespace LibraryApi.Query
{
    public class GetRentalQuery : IRequest<RentalResponse>
    {
        public int RentalId { get; init; }
    }
}