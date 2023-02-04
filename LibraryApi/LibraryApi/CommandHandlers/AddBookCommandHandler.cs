using System.Threading;
using System.Threading.Tasks;
using LibraryApi.Command;
using LibraryApi.Context;
using LibraryApi.Domain;
using MediatR;

namespace LibraryApi.CommandHandlers
{
    public class AddBookCommandHandler : IRequestHandler<AddBookCommand>
    {
        private readonly LibraryContext _context;

        public AddBookCommandHandler(LibraryContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book(request.Title, request.Author, request.Description, request.ImageUrl);

            await _context.Books.AddAsync(book, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}