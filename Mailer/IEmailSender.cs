using System.Threading.Tasks;

namespace Mailer
{
    public interface IEmailSender
    {
        string Email { get; set; }
        string From { get; set; }
        string HtmlMessage { get; set; }
        string Password { get; set; }
        string Subject { get; set; }

        Task SendEmailAsync(string to, string subject, string htmlMessage);
    }
}