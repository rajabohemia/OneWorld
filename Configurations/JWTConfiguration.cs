using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OneWorld.Configurations
{
    public class JWTConfiguration
    {
        public string Aud { get; set; }
        public string Iss { get; set; }
        public string Secret { get; set; }

        public double ExpireSeconds { get; set; }
        public SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));

        public SigningCredentials SigningCredentials =>
            new SigningCredentials(SymmetricSecurityKey, SecurityAlgorithms.HmacSha256);
    }
}