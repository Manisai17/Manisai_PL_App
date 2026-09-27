using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Manisai_PL_App.Services
{
    public class MailService : IMailService
    {
        private string _smtpServer;
        private string _smtpPort;
        private string _smtpUsername;
        private string _smtpPassword;

        private readonly IConfiguration _config;

        public MailService(IConfiguration config)
        {
            _config = config;

            _smtpServer = _config["SmtpSettings:SmtpServer"];
            _smtpPort = _config["SmtpSettings:SmtpPort"];
            _smtpUsername = _config["SmtpSettings:SmtpUsername"];
            _smtpPassword = _config["SmtpSettings:SmtpPassword"];
        }

        public string SendEmail(string sendername, string sendermail, string toname, string tomail, string subject, string content)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(sendername, sendermail));
            message.To.Add(new MailboxAddress(toname, tomail));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = content
            };

            using (var client = new SmtpClient())
            {
                client.Connect(_smtpServer, Convert.ToInt32(_smtpPort), SecureSocketOptions.StartTls);
                client.Authenticate(_smtpUsername, _smtpPassword);
                try
                {
                    client.Send(message);
                }
                catch (Exception e)
                {
                    return "Error in sending email :: " + e.Message;
                }
                client.Disconnect(true);
            }
            return "OK";
        }
    }
}