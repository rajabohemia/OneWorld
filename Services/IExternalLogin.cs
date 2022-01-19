using System.Threading.Tasks;
using OneWorld.DTO.Response;

namespace OneWorld.Services
{
    public interface IExternalLogin
    {
        Task<AccountLoginResponse> ExternalLoginCallbackAsync();
    }
}