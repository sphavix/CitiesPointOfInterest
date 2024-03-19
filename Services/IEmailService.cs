namespace CityPointOfInterest.Services
{
    public interface IEmailService
    {
        void Send(string subject, string message);
    }
}