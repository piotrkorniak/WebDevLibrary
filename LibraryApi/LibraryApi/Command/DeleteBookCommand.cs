using MediatR;

namespace LibraryApi.Command
{
    public class DeleteBookCommand : IRequest
    {
        public int Id { get; init; }
    }
}