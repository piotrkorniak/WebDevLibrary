using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Context;
using LibraryApi.DTO;
using LibraryApi.ExtensionMethods;
using LibraryApi.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.QueryHandler
{
    public class GetUserRentalsQueryHandler : IRequestHandler<GetUserRentalsQuery, IEnumerable<RentalResponse>>
    {
        private readonly LibraryContext _context;

        public GetUserRentalsQueryHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RentalResponse>> Handle(GetUserRentalsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var rentals = await _context.Rentals
                .Where(x => x.UserId == userId)
                .Include(x => x.Book)
                .Include(x => x.User)
                .ToArrayAsync(cancellationToken);

            var userRentalsResponse = rentals.Select(x => new RentalResponse
            {
                Id = x.Id,
                Status = x.Status.ToString(),
                StartDate = x.StartDate.ToTimestamp(),
                EndDate = x.EndDate.ToTimestamp(),
                Book = new BookResponse
                {
                    Id = x.Book.Id,
                    Author = x.Book.Author,
                    Title = x.Book.Title,
                    Description = x.Book.Description,
                    ImageUrl = x.Book.ImageUrl,
                    Status = x.Book.Status.ToString()
                },
                Rentee = new RenteeResponse
                {
                    Id = x.Book.Id,
                    Email = x.User.Email,
                    FirstName = x.User.FirstName,
                    LastName = x.User.LastName
                }
            });
            return userRentalsResponse;
        }
    }
}