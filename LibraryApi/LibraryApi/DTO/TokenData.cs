namespace LibraryApi.DTO
{
    public class TokenData
    {
        public string Token { get; init; }
        public long ExpirationTimestamp { get; init; }
    }
}