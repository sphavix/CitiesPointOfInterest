namespace CityPointOfInterest.Services
{
    public class EmailService : IEmailService
    {
        private string _mailTo = string.Empty;
        private string _mailFrom = string.Empty;

        public EmailService(IConfiguration configuration)
        {
            _mailTo = configuration["mailSettings:mailToAddress"];
            _mailFrom = configuration["mailSettings:mailFromAddress"];
        }

        public void Send(string subject, string message)
        {
            // send email - output to console for now
            Console.WriteLine($"Mail from {_mailFrom} to {_mailTo}, with {nameof(EmailService)}.");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Message: {message}");

        }
    }
}