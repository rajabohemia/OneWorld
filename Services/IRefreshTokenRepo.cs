using System.Threading.Tasks;
using OneWorld.Models;

namespace OneWorld.Services
{
    public interface IRefreshTokenRepo
    {
        Task<RefreshToken> AddAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetByRefreshTokenAsync(string refreshToken);
        Task<RefreshToken> DeleteRefreshTokenAsync(RefreshToken refreshToken);
    }
}