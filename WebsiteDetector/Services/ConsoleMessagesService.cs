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
                $"Введите 1, чтобы запустить процесс\n" +
                $"Введите 2, чтобы остановить процесс\n");
        }

        public void PublicateResults(WebsiteStatusResult statusResult)
        {
            Console.WriteLine($"Website: {statusResult.Url} Status: {statusResult.IsAvailable}");
        }

        public void ClearScreen()
        {
            Console.Clear();
        }
    }
}
