using System.Collections.Generic;
using System.Linq;
using LibraryApi.Enums;
using LibraryApi.Exceptions;
using LibraryApi.Infrastructure;

namespace LibraryApi.Domain
{
    public class Book
    {
        private Book()
        {
        }

        public Book(string title, string author, string description, string imageUrl)
        {
            Contract.Requires(IsTitleValid(title), ErrorCode.BookErrors.InvalidTitle);
            Contract.Requires(IsAuthorValid(author), ErrorCode.BookErrors.InvalidAuthor);
            Contract.Requires(IsDescriptionValid(description), ErrorCode.BookErrors.InvalidDescription);
            Contract.Requires(IsImageUrlValid(imageUrl), ErrorCode.BookErrors.InvalidImageUrl);

            Title = title;
            Author = author;
            Description = description;
            ImageUrl = imageUrl;
        }

        public int Id { get; }
        public string Title { get; }
        public string Author { get; }
        public string Description { get; }
        public string ImageUrl { get; }
        public BookStatus Status => IsBookRented() ? BookStatus.Unavailable : BookStatus.Available;
        public List<Rental> Rentals { get; }

        private static bool IsTitleValid(string title)
        {
            return !string.IsNullOrEmpty(title) && title.Length <= 100;
        }

        private static bool IsAuthorValid(string author)
        {
            return !string.IsNullOrEmpty(author) && author.Length <= 100;
        }

        private static bool IsDescriptionValid(string description)
        {
            return !string.IsNullOrEmpty(description) && description.Length <= 1000;
        }

        private static bool IsImageUrlValid(string imageUrl)
        {
            return !string.IsNullOrEmpty(imageUrl) && imageUrl.Length <= 1000;
        }

        public bool IsBookRented()
        {
            return Rentals.Any(x => !x.EndDate.HasValue);
        }

        public bool WasBookRented()
        {
            return Rentals.Count != 0;
        }
    }
}