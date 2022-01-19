using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OneWorld.DTO.Response;
using OneWorld.Models;

namespace OneWorld.Services
{
    public class ExternalLogin : IExternalLogin
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccountRepo _accountRepo;

        public ExternalLogin(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IAccountRepo accountRepo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _accountRepo = accountRepo;
        }

        public async Task<AccountLoginResponse> ExternalLoginCallbackAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info is null)
                return new AccountLoginResponse() {Errors = new[] {"Error From Providers."}};

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
            {
                return new AccountLoginResponse() {Errors = new[] {"Email Address Not Found."}};
            }

            var user = await _userManager.FindByEmailAsync(email);
            var signInResult =
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true, true);
            if (signInResult.Succeeded)
            {
                var result = await _accountRepo.GenerateJWTTokenAsync(user);
                return result;
            }

            if (user is null)
            {
                user = new ApplicationUser()
                {
                    Email = email,
                    UserName = email,
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(user);
                await _userManager.AddLoginAsync(user, info);
                var loginResult =
                    await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, true, true);
                if (!loginResult.Succeeded)
                {
                    return new AccountLoginResponse() {Errors = new[] {"Error Logging In."}};
                }

                var result = await _accountRepo.GenerateJWTTokenAsync(user);
                return result;
            }

            return new AccountLoginResponse() {Errors = new[] {"Something Went Wrong."}};
        }
    }
}