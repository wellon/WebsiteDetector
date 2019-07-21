namespace WebsiteDetector.Infrastructure
{
    public interface IAvailabilityDetectorService
    {
        void Start();
        void Stop();
    }
}