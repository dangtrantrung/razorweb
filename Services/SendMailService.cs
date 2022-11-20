using System;
using System.Threading.Tasks;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Microsoft.AspNetCore.Identity.UI.Services;

// Chứa thông tin Email sẽ gửi (Trường hợp này chưa hỗ trợ đính kém file)
public class MailContent
{
    public string To { get; set; }              // Địa chỉ gửi đến
    public string Subject { get; set; }         // Chủ đề (tiêu đề email)
    public string Body { get; set; }            // Nội dung (hỗ trợ HTML) của email

}
public class MailSettings
{
    public string Mail { get; set; }
    public string DisplayName { get; set; }
    public string Password { get; set; }
    public string Host { get; set; }
    public int Port { get; set; }

}

public class SendMailService : IEmailSender {
    private readonly MailSettings mailSettings;

    private readonly ILogger<SendMailService> logger;


    // mailSetting được Inject qua dịch vụ hệ thống
    // Có inject Logger để xuất log
    public SendMailService (IOptions<MailSettings> _mailSettings, ILogger<SendMailService> _logger) {
        mailSettings = _mailSettings.Value;
        logger = _logger;
        logger.LogInformation("Create SendMailService");
    }

    // Gửi email, theo nội dung trong mailContent
   
    public async Task SendEmailAsync(string email, string subject, string htmlMessage) {
       
       
        var message= new MimeMessage ();
        message.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
        message.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
        message.To.Add (MailboxAddress.Parse (email));
        message.Subject =subject;


        var builder = new BodyBuilder();
        builder.HtmlBody = htmlMessage;
       message.Body = builder.ToMessageBody ();

        // dùng SmtpClient của MailKit
        using var smtp = new MailKit.Net.Smtp.SmtpClient();

        try {
            smtp.Connect (mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate (mailSettings.Mail, mailSettings.Password);
            await smtp.SendAsync(message);
        }
        catch (Exception ex) {
            // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
            System.IO.Directory.CreateDirectory("mailssave");
            var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
            await message.WriteToAsync(emailsavefile);

            logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
            logger.LogError(ex.Message);
        }

        smtp.Disconnect (true);

        logger.LogInformation("send mail to " +email);
    }
}