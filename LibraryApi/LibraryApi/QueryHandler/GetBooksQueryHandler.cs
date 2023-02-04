using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Context;
using LibraryApi.DTO;
using LibraryApi.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.QueryHandler
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IEnumerable<BookResponse>>
    {
        private readonly LibraryContext _context;

        public GetBooksQueryHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookResponse>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var searchValue = request.SearchValue ?? "";

            var booksQuery = _context.Books
                .Include(x => x.Rentals)
                .Where(x => x.Author.Contains(searchValue) || x.Title.Contains(searchValue));

            if (request.IsAvailable) booksQuery = booksQuery.Where(x => x.Rentals.All(y => y.EndDate.HasValue));

            var books = await booksQuery.ToArrayAsync(cancellationToken);

            var booksResponse = books.Select(x => new BookResponse
            {
                Id = x.Id,
                Author = x.Author,
                Title = x.Title,
                Description = x.Description,
                ImageUrl = x.ImageUrl,
                Status = x.Status.ToString()
            });
            return booksResponse;
        }
    }
}