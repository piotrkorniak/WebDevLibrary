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
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly LibraryContext _context;

        public DeleteBookCommandHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var bookId = request.Id;

            var book = await _context.Books
                .Include(x => x.Rentals)
                .FirstOrDefaultAsync(x => x.Id == bookId, cancellationToken);

            Contract.Requires(book != null, ErrorCode.CommonErrors.NotFound);
            Contract.Requires(!book.WasBookRented(), ErrorCode.BookErrors.BookWasUse);

            _context.Books.Remove(book);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}