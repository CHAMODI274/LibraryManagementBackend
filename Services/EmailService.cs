using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Options;

public class EmailService
{
    private readonly EmailSettings _emailSettings;
    public EmailService(IOptions<EmailSettings> emailSettings) // in here we are doing a injection.  inject the EmailSettings from the IOptions
    {
        _emailSettings = emailSettings.Value;
    }
    public void SendEmail(string toEmail, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Support Student App", _emailSettings.SmtpUsername));
        message.To.Add(new MailboxAddress("Reciever Name", toEmail));
        message.Subject = subject;
        var textPart = new TextPart("plain")
        {
            Text = body
        };
        message.Body = textPart;

        using (var client = new SmtpClient())
        {
            client.Connect(_emailSettings.SmtpServer, _emailSettings.SmtpPort,
            SecureSocketOptions.StartTls);
            client.Authenticate(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
