using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UScheduler.WebApi.Tasks.Interfaces;
using UScheduler.WebApi.Tasks.Statics;

namespace UScheduler.WebApi.Tasks.Adapters
{
    public class BoardsAdapter : IBoardsAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BoardsAdapter> _logger;
        private readonly IConfiguration _configuration;

        public BoardsAdapter(
            HttpClient httpClient, 
            ILogger<BoardsAdapter> logger, 
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<(bool IsSuccess, string error)> BoardExists(Guid boardId)
        {
            try
            {
                var host = _configuration.GetValue<string>("MicroServicesEndpoints:Boards");

                var url = $"{host}/api/v1/Boards/{boardId}";
                var result = await _httpClient.GetAsync(new Uri(url));
                if (result.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                return result.StatusCode == HttpStatusCode.NotFound
                    ? (false, ErrorMessage.BoardNotFound)
                    : (false, result.ReasonPhrase);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, ex.Message);
            }
        }
    }
}
