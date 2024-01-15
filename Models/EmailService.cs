using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

public class EmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendEmailWithAttachment(string toEmail, string subject, string message, byte[] attachment)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("ahmad", "ahmadalzoubi099@gmail.com"));
        emailMessage.To.Add(new MailboxAddress("", toEmail));
        emailMessage.Subject = subject;

        var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = message;

        if (attachment != null)
        {
            bodyBuilder.Attachments.Add("message.pdf", attachment);
        }

        emailMessage.Body = bodyBuilder.ToMessageBody();

        using (var client = new SmtpClient())
        {
            client.Connect(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]), SecureSocketOptions.StartTls);
            client.Authenticate(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
            client.Send(emailMessage);
            client.Disconnect(true);
        }
    }
}



//using MailKit.Net.Smtp;
//using MimeKit;
//using MailKit.Security;
//using Microsoft.Extensions.Configuration;
//using System;

//public class EmailService
//{
//    private readonly IConfiguration _configuration;

//    public EmailService(IConfiguration configuration)
//    {
//        _configuration = configuration;
//    }

//    public void SendEmail(string? toEmail, string? subject, string? message)
//    {
//        var emailMessage = new MimeMessage();
//        emailMessage.From.Add(new MailboxAddress("ahmad", "ahmadalzoubi099@gmail.com"));
//        emailMessage.To.Add(new MailboxAddress("", toEmail));
//        emailMessage.Subject = subject;

//        var bodyBuilder = new BodyBuilder();
//        bodyBuilder.TextBody = message;
//        emailMessage.Body = bodyBuilder.ToMessageBody();

//        using (var client = new SmtpClient())
//        {
//            client.Connect(_configuration["Smtp:Host"], int.Parse(_configuration["Smtp:Port"]), SecureSocketOptions.StartTls);
//            client.Authenticate(_configuration["Smtp:Username"], _configuration["Smtp:Password"]);
//            client.Send(emailMessage);
//            client.Disconnect(true);
//        }
//    }
//}
