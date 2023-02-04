using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Context;
using LibraryApi.DTO;
using LibraryApi.Exceptions;
using LibraryApi.Infrastructure;
using LibraryApi.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.QueryHandler
{
    public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookResponse>
    {
        private readonly LibraryContext _context;

        public GetBookQueryHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<BookResponse> Handle(GetBookQuery request, CancellationToken cancellationToken)
        {
            var id = request.BookId;
            var book = await _context.Books.Where(x => x.Id == id)
                .Include(x => x.Rentals)
                .FirstOrDefaultAsync(cancellationToken);

            Contract.Requires(book != null, ErrorCode.CommonErrors.NotFound);

            var bookResponse = new BookResponse
            {
                Id = book.Id,
                Author = book.Author,
                Title = book.Title,
                Description = book.Description,
                ImageUrl = book.ImageUrl,
                Status = book.Status.ToString()
            };
            return bookResponse;
        }
    }
}