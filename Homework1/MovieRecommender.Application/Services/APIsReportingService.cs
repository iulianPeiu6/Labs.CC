using MovieRecommender.Application.Interfaces;
using MovieRecommender.Domain.Entities;

namespace MovieRecommender.Application.Services
{
    public class APIsReportingService : IAPIsReportingService
    {
        private readonly IRequestLogRepository requestLogs;

        public APIsReportingService(IRequestLogRepository requestLogs)
        {
            this.requestLogs = requestLogs;
        }
        public Report GenerateReport()
        {
            var requests = requestLogs.GetAll();

            var report = new Report
            {
                APIRequests = requests.Take(50),

                LatenciesForTMDb = requests
                    .Where(r => r.Provider == "TMDb")
                    .Select(r => r.Latency),

                LatenciesForYoutube = requests
                    .Where(r => r.Provider == "Youtube")
                    .Select(r => r.Latency),

                LatenciesForSendGrid = requests
                    .Where(r => r.Provider == "SendGrid")
                    .Select(r => r.Latency),

                MeanLatencyForTMDb = requests
                    .Where(r => r.Provider == "TMDb")
                    .Select(r => r.Latency)
                    .Average(),

                MeanLatencyForYoutube = requests
                    .Where(r => r.Provider == "Youtube")
                    .Select(r => r.Latency)
                    .Average(),

                MeanLatencyForSendGrid = requests
                    .Where(r => r.Provider == "SendGrid")
                    .Select(r => r.Latency)
                    .Average(),

                NumberOfRequestsOnTMDb = requests
                    .Where(r => r.Provider == "TMDb")
                    .Count(),

                NumberOfRequestsOnYoutube = requests
                    .Where(r => r.Provider == "Youtube")
                    .Count(),

                NumberOfRequestsOnSendGrid = requests
                    .Where(r => r.Provider == "SendGrid")
                    .Count(),

                NumberOfNotOkResponsesOnTMDb = requests
                    .Where(r => r.Provider == "TMDb" && r.StatusCode != "OK" && r.StatusCode != "Accepted")
                    .Count(),

                NumberOfNotOkResponsesOnYoutube = requests
                    .Where(r => r.Provider == "Youtube" && r.StatusCode != "OK" && r.StatusCode != "Accepted")
                    .Count(),

                NumberOfNotOkResponsesOnSendGrid = requests
                    .Where(r => r.Provider == "SendGrid" && r.StatusCode != "OK" && r.StatusCode != "Accepted")
                    .Count()
            };

            return report;
        }
    }
}
