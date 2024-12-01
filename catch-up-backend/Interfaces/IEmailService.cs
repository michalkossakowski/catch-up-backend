namespace catch_up_backend.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmail(
            string recipientEmail,
            string subject,
            string body,
            List<string>? attachments = null);
    }
}
