﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OneWorld.Configurations;
using OneWorld.DTO.Request;
using OneWorld.DTO.Response;
using OneWorld.Helpers;
using OneWorld.Models;
using OneWorld.ViewModels;

namespace OneWorld.Services
{
    public class AccountRepo : IAccountRepo
    {
        private readonly ILogger<AccountRepo> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTConfiguration _jwtConfiguration;
        private readonly CookieConfiguration _cookieConfiguration;
        private readonly IRefreshTokenRepo _refreshTokenRepo;

        public AccountRepo(ILogger<AccountRepo> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JWTConfiguration jwtConfiguration,
            CookieConfiguration cookieConfiguration,
            IRefreshTokenRepo refreshTokenRepo)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtConfiguration = jwtConfiguration;
            _cookieConfiguration = cookieConfiguration;
            _refreshTokenRepo = refreshTokenRepo;
        }

        public async Task<AccountLoginResponse> LoginAsync(AccountLoginVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (user is null)
                return new AccountLoginResponse() {Errors = new[] {"User Not Found."}};

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordCorrect)
                return new AccountLoginResponse() {Errors = new[] {"Invalid Credentials."}};

            var emailVerified = await _userManager.IsEmailConfirmedAsync(user);
            if (!emailVerified)
                return new AccountLoginResponse() {Errors = new[] {"Email Not Verified."}};

            var isSignedIn = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
            if (!isSignedIn.Succeeded)
                return new AccountLoginResponse() {Errors = new[] {"Error Signing In."}};

            return await GenerateJWTTokenAsync(user);
        }

        public async Task<AccountLoginResponse> RefreshTokenAsync(RefreshTokenRequest model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var tokenValidationParams = new TokenValidationParameters()
                {
                    ValidateLifetime =
                        false, //Keep it false here so that we can validate the other parameters of tokens otherwise it will throw exception
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidAudience = _jwtConfiguration.Aud,
                    ValidIssuer = _jwtConfiguration.Iss,
                    IssuerSigningKey = _jwtConfiguration.SymmetricSecurityKey
                };

                var principal = tokenHandler.ValidateToken(model.Token, tokenValidationParams, out var validatedToken);
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                            StringComparison.InvariantCultureIgnoreCase))
                    {
                        return new AccountLoginResponse() {Errors = new[] {"Invalid Token/Refresh Token."}};
                    }
                }

                var expirydateEpoch =
                    long.Parse(principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                var expiryDate = DateTimeOffset.FromUnixTimeSeconds(expirydateEpoch);
                if (expiryDate > DateTime.UtcNow)
                {
                    return new AccountLoginResponse() {Errors = new[] {"Token Not Expired."}};
                }

                var refreshTokenObj = await _refreshTokenRepo.GetByRefreshTokenAsync(model.RefreshToken);

                if (refreshTokenObj is null)
                    return new AccountLoginResponse() {Errors = new[] {"Refresh Token Doesn't Exist."}};

                if (DateTime.UtcNow > refreshTokenObj.ExpiryDate)
                    return new AccountLoginResponse() {Errors = new[] {"Refresh Token Expired. Please Login."}};

                if (refreshTokenObj.IsRevoked)
                    return new AccountLoginResponse() {Errors = new[] {"Refresh Token Revoked."}};

                var jti = principal.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (jti != refreshTokenObj.JwtId)
                    return new AccountLoginResponse() {Errors = new[] {"Invalid Tokens/Bad Tokens"}};

                //Delete The token before generating new
                await _refreshTokenRepo.DeleteRefreshTokenAsync(refreshTokenObj);
                var user = await _userManager.FindByIdAsync(refreshTokenObj.UserId);
                return await GenerateJWTTokenAsync(user);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return new AccountLoginResponse() {Errors = new[] {"Invalid Token or Something Went Wrong"}};
            }
        }

        private async Task<AccountLoginResponse> GenerateJWTTokenAsync(ApplicationUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            var claimsList = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, userRoles.FirstOrDefault() ?? ApplicationRoles.User.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp,
                    DateTimeOffset.UtcNow.AddSeconds(_jwtConfiguration.ExpireSeconds).ToUnixTimeSeconds().ToString())
            };

            var token = new JwtSecurityToken(issuer: _jwtConfiguration.Iss,
                audience: _jwtConfiguration.Aud,
                claims: claimsList,
                signingCredentials: _jwtConfiguration.SigningCredentials
            );

            var refreshToken = new RefreshToken
            {
                RefToken = $"{MiscActions.GenerateRandomString(35)}-{Guid.NewGuid()}",
                JwtId = token.Id,
                IsRevoked = false,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(_cookieConfiguration.ExpireDays),
                UserId = user.Id
            };

            await _refreshTokenRepo.AddAsync(refreshToken);
            return new AccountLoginResponse(true)
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.RefToken
            };
        }
    }
}