﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebsiteDetector.Configuration;
using WebsiteDetector.Infrastructure;
using WebsiteDetector.Models;

namespace WebsiteDetector.Services
{

    public class AvailabilityDetectorService : IAvailabilityDetectorService
    {
        private CancellationTokenSource cancellationTokenSource;
        private readonly IMessagesService messagesService;
        private readonly IList<string> websitesUrlsList;
        private readonly HttpClient client;
        private readonly int delay;

        public AvailabilityDetectorService(WebsitesConfig config, HttpClient client, IMessagesService messagesService)
        {
            this.websitesUrlsList = config.Websites.ToList();
            this.messagesService = messagesService;
            this.client = client;
            this.client.Timeout = TimeSpan.FromSeconds(config.TimeoutInSeconds);
            this.delay = config.DelayInMilliseconds;
        }

        public void Start()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            Task.Factory.StartNew(async (_) =>
            {
                while (!token.IsCancellationRequested)
                {
                    messagesService.ClearScreen();
                    messagesService.SendWelcomeMessage();
                    messagesService.SendWorkInProgressMessage();
                    await ProcessSiteCheck(token);
                }
            }, token, TaskCreationOptions.LongRunning);
        }

        private async Task ProcessSiteCheck(CancellationToken token)
        {
            var result = websitesUrlsList.Select(async check => await CheckIsAvailable(check, token)).ToList();
            var asd = await Task.WhenAll(result);
            
            foreach (var url in asd)
            {
                messagesService.PublicateResults(url);
            }

            await Task.Delay(delay);
        }

        private async Task<WebsiteStatusResult> CheckIsAvailable(string url, CancellationToken token)
        {
            try
            {
                using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, token))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        return new WebsiteStatusResult { Url = url, IsAvailable = true };
                    }
                    else
                    {
                        return new WebsiteStatusResult { Url = url, IsAvailable = false };
                    }
                }
            }
            catch
            {
                return new WebsiteStatusResult { Url = url, IsAvailable = false };
            }
        }

        public void Stop()
        {
            cancellationTokenSource.Cancel();
        }
    }
}