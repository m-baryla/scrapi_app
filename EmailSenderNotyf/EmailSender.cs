using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace scrapi.EmailSenderNotyf
{
    public static class EmailSender
    {
        public static void Send(string body,string to, string cc,string subject)
        {
            // create email message
            var email = new MimeMessage();
            var configJSON = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var emailValue = configJSON.GetSection("MailNotify");



            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;
            email.Body = bodyBuilder.ToMessageBody();
            email.Subject = subject;
            email.From.Add(new MailboxAddress(emailValue.GetValue<string>("from"), emailValue.GetValue<string>("from")));
            email.To.Add(new MailboxAddress(to,to));

            if (!string.IsNullOrEmpty(cc))
            {
                email.Cc.Add(new MailboxAddress(cc, cc));
            }

            // send email
            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(emailValue.GetValue<string>("authLog"), emailValue.GetValue<string>("authPass"));
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }
    }
}
