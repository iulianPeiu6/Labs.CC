using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UScheduler.WebApi.Boards.Interfaces;
using UScheduler.WebApi.Boards.Statics;

namespace UScheduler.WebApi.Boards.Adapters
{
    public class WorkspacesAdapter : IWorkspacesAdapter
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WorkspacesAdapter> _logger;
        private readonly IConfiguration _configuration;

        public WorkspacesAdapter(
            HttpClient httpClient,
            ILogger<WorkspacesAdapter> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<(bool IsSuccess, string Error)> WorkspaceExists(Guid workspaceId)
        {
            try
            {
                var host = _configuration.GetValue<string>("MicroServicesEndpoints:Workspaces");

                var url = $"{host}/api/v1/Workspaces/{workspaceId}";
                var result = await _httpClient.GetAsync(new Uri(url));
                if (result.IsSuccessStatusCode)
                {
                    return (true, null);
                }

                return result.StatusCode == HttpStatusCode.NotFound
                    ? (false, ErrorMessage.WorkspaceNotFound)
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
