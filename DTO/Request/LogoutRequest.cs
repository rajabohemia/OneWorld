using System.ComponentModel.DataAnnotations;

namespace OneWorld.DTO.Request
{
    public class LogoutRequest
    {
        [Required]
        public string RT { get; set; }
    }
}