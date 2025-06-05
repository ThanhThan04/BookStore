namespace BookStore.Smtp
{
    public interface ISmtp
    {
        Task SendEmailAsync(string toEmail, string subject, string body, bool isHtml = false);
    }
}
