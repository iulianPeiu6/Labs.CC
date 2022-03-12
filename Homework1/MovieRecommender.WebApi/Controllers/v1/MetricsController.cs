using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieRecommender.Application.Interfaces;
using MovieRecommender.Application.Services;
using MovieRecommender.Domain.Entities;

namespace MovieRecommender.WebApi.Controllers.v1
{
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MetricsController : ControllerBase
    {
        private readonly IAPIsReportingService reportingService;

        public MetricsController(IAPIsReportingService reportingService)
        {
            this.reportingService = reportingService;
        }

        [HttpGet]
        public async Task<Report> GetRecommendationsAsync()
        {
            var report = reportingService.GenerateReport();
            return report;
        }
    }
}
