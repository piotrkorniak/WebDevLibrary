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
    public class GetRentalQueryHandler : IRequestHandler<GetRentalQuery, RentalResponse>
    {
        private readonly LibraryContext _context;

        public GetRentalQueryHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<RentalResponse> Handle(GetRentalQuery request, CancellationToken cancellationToken)
        {
            var id = request.RentalId;
            var rental = await _context.Rentals.Where(x => x.Id == id)
                .Include(x => x.User)
                .Include(x => x.Book)
                .FirstOrDefaultAsync(cancellationToken);


            Contract.Requires(rental != null, ErrorCode.CommonErrors.NotFound);

            var rentalResponse = new RentalResponse
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
            return rentalResponse;
        }
    }
}