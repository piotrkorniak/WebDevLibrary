using System;
using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Command;
using LibraryApi.Context;
using LibraryApi.Exceptions;
using LibraryApi.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.CommandHandlers
{
    public class AddUserRentalCommandHandler : IRequestHandler<AddUserRentalCommand>
    {
        private readonly LibraryContext _context;

        public AddUserRentalCommandHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddUserRentalCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId == null) throw new Exception("User is not defined.");

            var book = await _context.Books
                .Include(x => x.Rentals)
                .FirstOrDefaultAsync(x => x.Id == request.BookId,
                    cancellationToken);

            Contract.Requires(book != null, ErrorCode.RentalErrors.BookIsNull);
            Contract.Requires(book != null && !book.IsBookRented(), ErrorCode.RentalErrors.BookInUse);

            var user = await _context.Users
                .Include(x => x.Rentals)
                .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

            user.RentBook(book);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}