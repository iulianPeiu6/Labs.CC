using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MovieRecommender.Application.Interfaces;
using Newtonsoft.Json;
using Youtube.Options;
using Youtube.Services.Abstracts;

namespace Youtube.Services
{
    public class YoutubeService : YoutubeServiceBase, IYoutubeService
    {
        public YoutubeService(IOptions<YoutubeConfiguration> config, IRequestLogRepository requestLogs) : base(config, requestLogs)
        {
        }

        public async Task<string> GetFirstSearchVideoLinkAsync(string searchText)
        {
            var query = new Dictionary<string, string>()
            {
                ["part"] = "snippet",
                ["q"] = searchText,
                ["key"] = config.ApiKey
            };

            var url = QueryHelpers.AddQueryString(config.WebApiEndpoint.SearchV3, query);

            var request = new HttpRequestMessage(HttpMethod.Get, url);

            var response = await MakeRequest(request);

            var parsedResponse = JsonConvert.DeserializeObject<dynamic>(response);

            var videoId = string.Empty;
            try
            {
                videoId = parsedResponse?.items[0]?.id?.videoId;
            }
            catch
            {
                // Ignored
            }

            return $"{config.WebUiEndpoint.Watch}?v={videoId}";
        }
    }
}
