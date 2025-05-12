using System.Threading.Tasks;

namespace BoxBuildproj.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
    }
}
