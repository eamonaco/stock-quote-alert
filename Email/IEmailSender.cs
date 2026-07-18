using Settings;

namespace Email;

public interface IEmailSender
{
    Task SendEmail(EmailSettings config, string assunto, string corpo);
}