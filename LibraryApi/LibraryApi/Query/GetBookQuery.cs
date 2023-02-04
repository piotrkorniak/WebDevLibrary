using LibraryApi.DTO;
using MediatR;

namespace LibraryApi.Query
{
    public class GetBookQuery : IRequest<BookResponse>
    {
        public int BookId { get; init; }
    }
}