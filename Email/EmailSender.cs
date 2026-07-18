using Settings;
using System.Net;
using System.Net.Mail;
using EmailInterface;

namespace Email;

public class EmailSender : IEmailSender {
    public async Task SendEmail(EmailSettings config, string assunto, string corpo)
    {
        using var mensagem = new MailMessage();

        mensagem.From = new MailAddress(config.From);
        mensagem.To.Add(config.EmailTo);
        mensagem.Subject = assunto;
        mensagem.Body = corpo;
        mensagem.IsBodyHtml = false;

        using var smtp = new SmtpClient(config.SmtpHost, config.SmtpPort);

        smtp.Credentials = new NetworkCredential(
            config.Username,
            config.Password);

        smtp.EnableSsl = config.EnableSsl;

        await smtp.SendMailAsync(mensagem);
    }

}
