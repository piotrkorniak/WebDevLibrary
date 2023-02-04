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
    public class CloseRentalCommandHandler : IRequestHandler<CloseRentalCommand>
    {
        private readonly LibraryContext _context;

        public CloseRentalCommandHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(CloseRentalCommand request, CancellationToken cancellationToken)
        {
            var rentalId = request.RentalId;

            var rental =
                await _context.Rentals.FirstOrDefaultAsync(x => x.Id == rentalId, cancellationToken);

            Contract.Requires(!rental.IsCanceled(), ErrorCode.RentalErrors.RentalIsCanceled);
            Contract.Requires(!rental.IsCompleted(), ErrorCode.RentalErrors.RentalIsCompleted);

            rental.CloseRental();
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}