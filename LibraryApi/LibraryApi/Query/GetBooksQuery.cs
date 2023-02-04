using System.Collections.Generic;
using LibraryApi.DTO;
using MediatR;

namespace LibraryApi.Query
{
    public class GetBooksQuery : IRequest<IEnumerable<BookResponse>>
    {
        public string SearchValue { get; init; }
        public bool IsAvailable { get; init; }
    }
}