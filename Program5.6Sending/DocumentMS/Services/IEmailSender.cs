using DocumentMS.Models.EmailConfigViewModel;
using System.Threading.Tasks;

namespace DocumentMS.Services
{
    public interface IEmailSender
    {
        Task<Task> SendEmailAsync(string email, string subject, string message);
        Task<Task> SendEmailByGmailAsync(SendEmailViewModel vm);
    }
}
