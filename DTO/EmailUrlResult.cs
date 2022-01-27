using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace OneWorld.DTO
{
    public class EmailUrlResult : BaseErrorSuccess
    {
        public EmailUrlResult()
        {
        }
        
        public EmailUrlResult(bool issuccess)
        {
            Success = issuccess;
        }

        public string EmailConfirmationUrl { get; set; }
    }
}