using LibraryApi.Exceptions;

namespace LibraryApi.Infrastructure
{
    public static class Contract
    {
        public static void Requires(bool condition, ErrorCode errorCode)
        {
            if (!condition) throw new CustomException(errorCode);
        }
    }
}