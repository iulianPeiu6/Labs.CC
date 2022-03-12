using Microsoft.Extensions.Options;
using MovieRecommender.Application.Interfaces;
using MovieRecommender.Domain.Entities;
using System.Diagnostics;
using Youtube.Options;

namespace Youtube.Services.Abstracts
{
    public abstract class YoutubeServiceBase
    {
        protected readonly YoutubeConfiguration config;
        protected readonly HttpClient client;
        private readonly IRequestLogRepository requestLogs;

        public YoutubeServiceBase(IOptions<YoutubeConfiguration> config, IRequestLogRepository requestLogs)
        {
            this.config = config.Value;
            client = new HttpClient();
            this.requestLogs = requestLogs;
        }

        protected async Task<string> MakeRequest(HttpRequestMessage request)
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
                Provider = "Youtube",
                RequestContent = request.Content?.ToString(),
                StatusCode = response.StatusCode.ToString(),
                ResponseContent = responseContent,
                Latency = (int)watch.ElapsedMilliseconds
            };
            requestLogs.Add(requestLog);

            return responseContent;
        }
    }
}
