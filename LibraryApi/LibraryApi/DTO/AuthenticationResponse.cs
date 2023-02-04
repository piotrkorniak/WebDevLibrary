namespace LibraryApi.DTO
{
    public class AuthenticationResponse
    {
        public int Id { get; init; }
        public string Email { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Role { get; init; }
        public string Token { get; init; }
        public long TokenExpirationDate { get; init; }
    }
}