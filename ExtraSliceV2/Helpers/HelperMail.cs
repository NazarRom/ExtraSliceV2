using System.Net;
using System.Net.Mail;

namespace ExtraSliceV2.Helpers
{
    public class HelperMail
    {
        private IConfiguration configuration;
        public HelperMail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        private MailMessage ConfigureMailMessege
            (string para, string asunto, string mensaje)
        {
            MailMessage mailMessage = new MailMessage();
            string email = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            mailMessage.From = new MailAddress(email);
            mailMessage.To.Add(new MailAddress(para));
            mailMessage.Subject = asunto;
            mailMessage.Body = mensaje;
            mailMessage.IsBodyHtml = true;
            return mailMessage;
        }

        private SmtpClient ConfigureSmtpClient()
        {
            string user = this.configuration.GetValue<string>("MailSettings:Credentials:User");
            string password = this.configuration.GetValue<string>("MailSettings:Credentials:Password");
            string host = this.configuration.GetValue<string>("MailSettings:Smtp:Host");
            int port = this.configuration.GetValue<int>("MailSettings:Smtp:Port");
            bool enableSSL = this.configuration.GetValue<bool>("MailSettings:Smtp:EnableSSL");
            bool defaultCredetials = this.configuration.GetValue<bool>("MailSettings:Smtp:DefaultCredentials");
            SmtpClient client = new SmtpClient();
            client.Host = host;
            client.Port = port;
            client.EnableSsl = enableSSL;
            client.UseDefaultCredentials = defaultCredetials;

            NetworkCredential credentials = new NetworkCredential(user, password);
            client.Credentials = credentials;
            return client;
        }

        public async Task SendMailAsync(string para, string asunto, string mensaje)
        {
            MailMessage mail = this.ConfigureMailMessege(para, asunto, mensaje);
            SmtpClient client = this.ConfigureSmtpClient();
            await client.SendMailAsync(mail);
        }


       
    }
}
