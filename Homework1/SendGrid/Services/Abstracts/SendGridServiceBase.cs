using Microsoft.Extensions.Options;
using MovieRecommender.Application.Interfaces;
using MovieRecommender.Domain.Entities;
using SendGrid.Options;
using System.Diagnostics;

namespace SendGrid.Services.Abstracts
{
    public abstract class SendGridServiceBase
    {
        protected readonly SendGridConfiguration config;
        protected readonly HttpClient client;
        private readonly IRequestLogRepository requestLogs;

        public SendGridServiceBase(IOptions<SendGridConfiguration> config, IRequestLogRepository requestLogs)
        {
            this.config = config.Value;
            this.client = new HttpClient();
            this.requestLogs = requestLogs;
        }

        protected async Task<bool> MakeRequest(HttpRequestMessage request)
        {
            Stopwatch watch = new Stopwatch();

            watch.Start();
            var response = await client.SendAsync(request);
            watch.Stop();

            var responseContent = await response.Content.ReadAsStringAsync();

            var requestLog = new RequestLog
            {
                Endpoint = request.RequestUri.ToString(),
                HttpMethod = request.Method.Method,
                Provider = "SendGrid",
                RequestContent = request.Content?.ToString(),
                StatusCode = response.StatusCode.ToString(),
                ResponseContent = responseContent,
                Latency = (int)watch.ElapsedMilliseconds
            };
            requestLogs.Add(requestLog);

            return response.IsSuccessStatusCode;
        }
    }
}