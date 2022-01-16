using System.ComponentModel.DataAnnotations;

namespace OneWorld.DTO.Request
{
    public class RefreshTokenRequest
    {
        [Required] public string Token { get; set; }
        [Required] public string RefreshToken { get; set; }
    }
}