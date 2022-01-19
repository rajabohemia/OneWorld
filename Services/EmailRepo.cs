using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace OneWorld.Services
{
    public class EmailRepo : IEmailRepo
    {
        private readonly ILogger<EmailRepo> _logger;

        public EmailRepo(ILogger<EmailRepo> logger)
        {
            _logger = logger;
        }
        
        public async Task<bool> SendEmailConfirmationAsync(string email, string url)
        {
            _logger.LogCritical(url);
            return true;
        }
    }
}