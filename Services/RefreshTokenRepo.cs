using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OneWorld.Models;

namespace OneWorld.Services
{
    public class RefreshTokenRepo : IRefreshTokenRepo
    {
        private readonly AppDbContext _appDbContext;

        public RefreshTokenRepo(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<RefreshToken> AddAsync(RefreshToken refreshToken)
        {
            var result = _appDbContext.RefreshTokens.Attach(refreshToken);
            result.State = EntityState.Added;
            await _appDbContext.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<RefreshToken> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _appDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.RefToken == refreshToken);
        }

        public async Task<RefreshToken> DeleteRefreshTokenAsync(RefreshToken refreshToken)
        {
            _appDbContext.RefreshTokens.Remove(refreshToken);
            await _appDbContext.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<bool> DeleteRefreshTokenByRefreshTokenAsync(string RefreshToken)
        {
            var RefToken = await _appDbContext.RefreshTokens.FirstOrDefaultAsync(x => x.RefToken == RefreshToken);
            if (RefToken is not null)
            {
                _appDbContext.RefreshTokens.Remove(RefToken);
                await _appDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}