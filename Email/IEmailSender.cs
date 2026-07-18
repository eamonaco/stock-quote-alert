using Settings;

namespace EmailInterface;

public interface IEmailSender
{
    Task SendEmail(EmailSettings config, string assunto, string corpo);
}