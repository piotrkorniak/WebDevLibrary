namespace LibraryApi.Exceptions
{
    public class ErrorCode
    {
        private ErrorCode(string category, string code)
        {
            Value = $"ApiError.{category}.{code}";
        }

        public string Value { get; }


        public static class CommonErrors
        {
            private const string CategoryName = "Common";
            public static ErrorCode InternalError => new(CategoryName, "InternalError");
            public static ErrorCode NotFound => new(CategoryName, "NotFound");
        }

        public static class UserErrors
        {
            private const string CategoryName = "User";
            public static ErrorCode InvalidEmail => new(CategoryName, "InvalidEmail");
            public static ErrorCode InvalidPassword => new(CategoryName, "InvalidPassword");
            public static ErrorCode InvalidFirstName => new(CategoryName, "InvalidFirstName");
            public static ErrorCode InvalidLastName => new(CategoryName, "InvalidLastName");
        }

        public static class RegisterErrors
        {
            private const string CategoryName = "Register";
            public static ErrorCode EmailInUse => new(CategoryName, "EmailInUse");
        }

        public static class LoginErrors
        {
            private const string CategoryName = "Login";
            public static ErrorCode InvalidCredentials => new(CategoryName, "InvalidCredentials");
        }

        public static class BookErrors
        {
            private const string CategoryName = "Book";
            public static ErrorCode InvalidTitle => new(CategoryName, "InvalidTitle");
            public static ErrorCode InvalidAuthor => new(CategoryName, "InvalidAuthor");
            public static ErrorCode InvalidDescription => new(CategoryName, "InvalidDescription");
            public static ErrorCode InvalidImageUrl => new(CategoryName, "InvalidImageUrl");
            public static ErrorCode BookWasUse => new(CategoryName, "BookWasUse");
        }

        public static class RentalErrors
        {
            private const string CategoryName = "Rental";
            public static ErrorCode BookInUse => new(CategoryName, "BookInUse");
            public static ErrorCode BookIsNull => new(CategoryName, "BookIsNull");
            public static ErrorCode RentalIsNotPending => new(CategoryName, "RentalIsNotPending");
            public static ErrorCode RentalIsCompleted => new(CategoryName, "RentalIsCompleted");
            public static ErrorCode RentalIsCanceled => new(CategoryName, "RentalIsCanceled");
        }
    }
}