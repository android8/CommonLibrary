using Microsoft.Extensions.Configuration;

namespace Mailer
{
   public interface IMailerConfiguration
   {
      public string SmtpServer { get; }
      public int SmptPort { get; }
      public string EnableSSL { get; }
      public string DefaultRecipient { get; }
      public string Sender { get; }
      public IConfigurationSection GetConfigurationSection(string Key);
   }
}
