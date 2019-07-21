using System;
using WebsiteDetector.Infrastructure;
using WebsiteDetector.Models;

namespace WebsiteDetector.Services
{
    class ConsoleMessagesService : IMessagesService
    {
        public void SendWelcomeMessage()
        {
            Console.WriteLine(
                $"Enter 1, to start process\n" +
                $"Enter 2, to stop process\n");
        }

        public void SendWorkInProgressMessage()
        {
            Console.WriteLine(
                $"Checking websites availability...");
        }

        public void PublicateResults(WebsiteStatusResult statusResult)
        {
            Console.BackgroundColor = statusResult.IsAvailable ? ConsoleColor.Green : ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"Website: {statusResult.Url} | IsAvailable: {statusResult.IsAvailable}");
            Console.ResetColor();
        }

        public void ClearScreen()
        {
            Console.Clear();
        }
    }
}
