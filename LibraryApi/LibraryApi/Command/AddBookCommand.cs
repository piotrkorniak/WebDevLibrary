using MediatR;

namespace LibraryApi.Command
{
    public class AddBookCommand : IRequest
    {
        public string Title { get; init; }
        public string Author { get; init; }
        public string Description { get; init; }
        public string ImageUrl { get; init; }
    }
}