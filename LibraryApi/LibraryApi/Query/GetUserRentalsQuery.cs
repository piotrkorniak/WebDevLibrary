using System.Collections.Generic;
using LibraryApi.DTO;
using MediatR;

namespace LibraryApi.Query
{
    public class GetUserRentalsQuery : IRequest<IEnumerable<RentalResponse>>
    {
        public int UserId { get; init; }
    }
}