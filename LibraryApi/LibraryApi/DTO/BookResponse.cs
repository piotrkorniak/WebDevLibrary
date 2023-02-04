namespace LibraryApi.DTO
{
    public class BookResponse
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Author { get; init; }
        public string Description { get; init; }
        public string ImageUrl { get; init; }

        public string Status { get; init; }
    }
}