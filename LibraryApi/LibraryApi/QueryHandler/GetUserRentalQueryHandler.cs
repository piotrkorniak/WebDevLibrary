using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Context;
using LibraryApi.DTO;
using LibraryApi.Exceptions;
using LibraryApi.ExtensionMethods;
using LibraryApi.Infrastructure;
using LibraryApi.Query;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.QueryHandler
{
    public class GetUserRentalQueryHandler : IRequestHandler<GetUserRentalQuery, RentalResponse>
    {
        private readonly LibraryContext _context;

        public GetUserRentalQueryHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<RentalResponse> Handle(GetUserRentalQuery request,
            CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var rentalId = request.RentalId;
            var rental = await _context.Rentals
                .Where(x => x.UserId == userId && x.Id == rentalId)
                .Include(x => x.Book)
                .Include(x => x.User)
                .FirstOrDefaultAsync(cancellationToken);

            Contract.Requires(rental != null, ErrorCode.CommonErrors.NotFound);
            var userRentalResponse = new RentalResponse
            {
                Id = rental.Id,
                Status = rental.Status.ToString(),
                StartDate = rental.StartDate.ToTimestamp(),
                EndDate = rental.EndDate.ToTimestamp(),
                Book = new BookResponse
                {
                    Id = rental.Book.Id,
                    Author = rental.Book.Author,
                    Title = rental.Book.Title,
                    Description = rental.Book.Description,
                    ImageUrl = rental.Book.ImageUrl,
                    Status = rental.Book.Status.ToString()
                },
                Rentee = new RenteeResponse
                {
                    Id = rental.Book.Id,
                    Email = rental.User.Email,
                    FirstName = rental.User.FirstName,
                    LastName = rental.User.LastName
                }
            };
            return userRentalResponse;
        }
    }
}