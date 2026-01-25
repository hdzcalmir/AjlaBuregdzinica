using BuregdzinicaAjla.Model;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
namespace BuregdzinicaAjla.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailForm emailForm);
    }
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<MailSettings> mailSettings, ILogger<EmailSender> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;

        }

        //public async Task SendEmailAsync(EmailForm emailForm)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Buregdžinica Ajla – Web forma", _mailSettings.SenderEmail));
        //    message.To.Add(new MailboxAddress("", emailForm.Email));
        //    //message.ReplyTo.Add(MailboxAddress.Parse(emailForm.Email));
        //    message.Subject = emailForm.Naslov;
        //    message.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = emailForm.Poruka };

        //    using var client = new SmtpClient();

        //    try
        //    {
        //        await client.ConnectAsync(
        //            _mailSettings.SmtpServer,
        //            _mailSettings.Port,
        //            MailKit.Security.SecureSocketOptions.StartTls);

        //        await client.AuthenticateAsync(
        //            _mailSettings.Username,
        //            _mailSettings.Password);

        //        await client.SendAsync(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Email sending failed.");
        //        throw;
        //    }
        //    finally
        //    {
        //        await client.DisconnectAsync(true);
        //    }
        //}
        public async Task SendEmailAsync(EmailForm emailForm)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Buregdžinica Ajla – Web forma", _mailSettings.SenderEmail));
            message.To.Add(MailboxAddress.Parse(_mailSettings.ReceiverEmail ?? _mailSettings.SenderEmail));
            message.ReplyTo.Add(MailboxAddress.Parse(emailForm.Email));
            message.Subject = $"Kontakt forma: {emailForm.Naslov}";

            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
            <p><strong>Ime i prezime:</strong> {emailForm.ImePrezime}</p>
            <p><strong>Email:</strong> {emailForm.Email}</p>
            <p><strong>Naslov:</strong> {emailForm.Naslov}</p>
            <p><strong>Poruka:</strong><br>{emailForm.Poruka}</p>"
            };

            using var client = new SmtpClient();

            try
            {
                await client.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_mailSettings.Username, _mailSettings.Password);
                await client.SendAsync(message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

    }
}
