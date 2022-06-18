using Core.Interface;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Core.Entity;
using System.Text;

namespace Core.Service
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;
        private readonly IOptions<MailSettings> mail;
        public MailService(IOptions<MailSettings> mail)
        {
            this.mail = mail;
            _mailSettings = mail.Value;
        }
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse("solido1a@gmx.de"));
            email.Subject = mailRequest.Subject;
            StringBuilder sb = new StringBuilder();
            var builder = new BodyBuilder();
            sb.AppendLine("Name des Benutzers -> " + mailRequest.Name + " |");
            sb.AppendLine("Benutzertelefon -> " + mailRequest.userPhone + " |");
            sb.AppendLine("Kommentar -> " + mailRequest.Body);
            builder.HtmlBody = sb.ToString();
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
