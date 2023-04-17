using Microsoft.Extensions.Configuration;
using System;

namespace Mailer
{
   /// <summary>
   /// register this class at a CORE web app IoC container at Startup.ConfigureServices()
   /// get mailer configuration from appSetting
   /// </summary>
   public class MailerConfiguration : IMailerConfiguration
   {
      private readonly IConfiguration _configuration;
      public MailerConfiguration(IConfiguration configuration)
      {
         _configuration = configuration;
      }

      public string SmtpServer => _configuration["EmailSender:Host"];

      public int SmptPort => int.Parse(_configuration["EmailSender:Port"]);

      public string EnableSSL => _configuration["EmailSender:EnableSSL"];

      public string DefaultRecipient => _configuration["EmailSender:DefaultRecipient"];

      public string Sender => _configuration["EmailSender:Sender"];

      IConfigurationSection IMailerConfiguration.GetConfigurationSection(string key)
      {
         return _configuration.GetSection(key);
      }
   }
}
