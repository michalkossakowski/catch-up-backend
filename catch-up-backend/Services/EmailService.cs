using System.Net.Mail;
using System.Net;
using catch_up_backend.Interfaces;

namespace catch_up_backend.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _senderEmail = "foruminformatyczneasp@gmail.com";
        private readonly string _senderPassword = "ywey xtxj emfw xmjv";

        public async Task SendEmail(
            string recipientEmail,
            string subject,
            string body,
            List<string>? attachments = null)
        {
            try
            {
                // Tworzenie wiadomości
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(_senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true // Umożliwia wysyłanie wiadomości w formacie HTML
                };
                mail.To.Add(recipientEmail);
                // Dodawanie załączników
                if (attachments != null)
                {
                    foreach (string attachmentPath in attachments)
                    {
                        if (File.Exists(attachmentPath))
                        {
                            mail.Attachments.Add(new Attachment(attachmentPath));
                        }
                        else
                        {
                            Console.WriteLine($"Attachment {attachmentPath} does not exists.");
                        }
                    }
                }

                // Konfiguracja klienta SMTP
                using SmtpClient smtpClient = new SmtpClient(_smtpServer, _smtpPort)
                {
                    Credentials = new NetworkCredential(_senderEmail, _senderPassword),
                    EnableSsl = true // Włączenie szyfrowania SSL/TLS
                };

                // Wysyłanie e-maila
                await smtpClient.SendMailAsync(mail);
            }
            catch (Exception ex)
            {
                throw; // Ponownie wyrzucam wyjątek, aby móc obsłużyć go na wyższym poziomie
            }
        }
    }
}
