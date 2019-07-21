namespace WebsiteDetector.Configuration
{
    public class WebsitesConfig
    {
        public string[] Websites { get; set; }
        public int TimeoutInSeconds { get; set; }
        public int DelayInMilliseconds { get; set; }
    }
}
