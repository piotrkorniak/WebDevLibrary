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
    public class CloseUserRentalCommandHandler : IRequestHandler<CloseUserRentalCommand>
    {
        private readonly LibraryContext _context;

        public CloseUserRentalCommandHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CloseUserRentalCommand request, CancellationToken cancellationToken)
        {
            var rentalId = request.RentalId;
            var userId = request.UserId;

            var rental =
                await _context.Rentals.FirstOrDefaultAsync(x => x.Id == rentalId && x.UserId == userId,
                    cancellationToken);

            Contract.Requires(rental != null, ErrorCode.CommonErrors.NotFound);
            Contract.Requires(rental.IsPending(), ErrorCode.RentalErrors.RentalIsNotPending);

            rental.CloseRental();
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}