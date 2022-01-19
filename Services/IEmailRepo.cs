using System.Threading.Tasks;

namespace OneWorld.Services
{
    public interface IEmailRepo
    {
        Task<bool> SendEmailConfirmationAsync(string email, string url);
    }
}