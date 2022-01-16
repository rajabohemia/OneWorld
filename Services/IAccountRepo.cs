using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OneWorld.DTO.Request;
using OneWorld.DTO.Response;
using OneWorld.ViewModels;

namespace OneWorld.Services
{
    public interface IAccountRepo
    {
        Task<AccountLoginResponse> LoginAsync(AccountLoginVM model);
        Task<AccountLoginResponse> RefreshTokenAsync(RefreshTokenRequest model);
    }
}