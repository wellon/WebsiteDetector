using WebsiteDetector.Models;

namespace WebsiteDetector.Infrastructure
{
    public interface IMessagesService
    {
        void SendWelcomeMessage();
        void SendWorkInProgressMessage();
        void PublicateResults(WebsiteStatusResult statusResult);
        void ClearScreen();
    }
}
