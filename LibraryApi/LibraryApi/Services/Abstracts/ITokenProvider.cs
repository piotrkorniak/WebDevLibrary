using LibraryApi.DTO;
using LibraryApi.Enums;

namespace LibraryApi.Services.Abstracts
{
    public interface ITokenProvider
    {
        TokenData Create(int userId, UserRole userRole);
    }
}