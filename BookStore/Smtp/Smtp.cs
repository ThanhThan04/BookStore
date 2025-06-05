using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;

namespace BookStore.Smtp
{
    public class Smtp:ISmtp
    {
        private readonly SmtpSetting _smtpSettings;

        public Smtp(IOptions<SmtpSetting> smtpOptions)
        {
            _smtpSettings = smtpOptions.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false)
        {
            var smtpClient = new SmtpClient(_smtpSettings.Host)
            {
                Port = _smtpSettings.Port,
                Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
                EnableSsl = _smtpSettings.EnableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}

