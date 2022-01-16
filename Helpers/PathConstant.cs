using System.Resources;
using System.Security.Claims;

namespace OneWorld.Helpers
{
    public class PathConstant
    {
        public const string API = "API";
        public const string Admin = "Admin";

        public static class ApiVersion
        {
            public const string v1 = "v1";
            public const string api = "api";
        }

        public static class ApiPath
        {
            public const string Login = "Account/Login";
            public const string RefreshToken = "Account/RefreshToken";
        }
        
        public static class cookies
        {
            public const string RefreshToken = "RT";
            public const string JWTAuthToken = "Token";
        }
    }
}