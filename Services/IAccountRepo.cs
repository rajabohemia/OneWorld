using System.Runtime.InteropServices;
using System.Threading.Tasks;
using OneWorld.Areas.Admin.ViewModels;
using OneWorld.DTO;
using OneWorld.DTO.Request;
using OneWorld.DTO.Response;
using OneWorld.Models;
using OneWorld.ViewModels;

namespace OneWorld.Services
{
    public interface IAccountRepo
    {
        Task<AccountLoginResponse> GenerateJWTTokenAsync(ApplicationUser applicationUser);
        Task<AccountLoginResponse> LoginAsync(AccountLoginVM model);
        Task<AccountLoginResponse> RefreshTokenAsync(RefreshTokenRequest model);
        Task<AccountRegisterResult> RegisterAsync(AccountRegisterVM model);
        Task<BaseErrorSuccess> ConfirmEmailAsync(string email, string base64Token);
        Task<BaseErrorSuccess> ForgotPasswordAsync(AccountForgotPasswordVM model);
        Task<BaseErrorSuccess> LogoutAsync(LogoutRequest model);
        Task<EmailUrlResult> ResendEmailConfirmationAsync(AccountResendEmailConfirmation model);
        Task<BaseErrorSuccess> ChangePasswordAsync(AccountChangePasswordVM model);
    }
}