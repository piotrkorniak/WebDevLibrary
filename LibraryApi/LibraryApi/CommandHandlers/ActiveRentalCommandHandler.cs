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
    public class ActiveRentalCommandHandler : IRequestHandler<ActiveRentalCommand>
    {
        private readonly LibraryContext _context;

        public ActiveRentalCommandHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ActiveRentalCommand request, CancellationToken cancellationToken)
        {
            var rentalId = request.RentalId;

            var rental =
                await _context.Rentals.FirstOrDefaultAsync(x => x.Id == rentalId, cancellationToken);

            Contract.Requires(rental != null, ErrorCode.CommonErrors.NotFound);
            Contract.Requires(rental.IsPending(), ErrorCode.RentalErrors.RentalIsNotPending);

            rental.ActivateRental();
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}