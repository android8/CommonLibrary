using System.Net.Mail;
using System.Threading.Tasks;

namespace Mailer
{
  /// <summary>
  /// implement CORE Identity UI Services and inject IMailerConfiguration to read settings from web app's appSettings.json
  /// </summary>
  public class EmailSender : IEmailSender
  {
    private readonly IMailerConfiguration _mailerConfiguration;
    // Our private configuration variables
    private readonly string Host;
    private readonly int Port;

    public string From { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string HtmlMessage { get; set; }

    /// <summary>
    /// get smpt server from web app's appSettings.json
    /// </summary>
    /// <param name="mailerConfiguration"></param>
    public EmailSender(IMailerConfiguration mailerConfiguration)
    {
      _mailerConfiguration = mailerConfiguration;
      Host = _mailerConfiguration.SmtpServer;
      Port = _mailerConfiguration.SmptPort;
      From = _mailerConfiguration.Sender;
    }

    // Use our configuration to send the email by using SmtpClient
    public Task SendEmailAsync(string to, string subject, string htmlMessage)
    {
      var client = new SmtpClient(Host, Port);
      //{
      //Credentials = new NetworkCredential(userName, password),
      //EnableSsl = enableSSL
      //};
      return client.SendMailAsync(
          new MailMessage(From, to, subject, htmlMessage) { IsBodyHtml = true }
      );
    }
  }
}
