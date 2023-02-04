using System.Collections.Generic;
using LibraryApi.DTO;
using MediatR;

namespace LibraryApi.Query
{
    public class GetAllRentalsQuery : IRequest<IEnumerable<RentalResponse>>
    {
    }
}