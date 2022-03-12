using Bogus.DataSets;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MovieRecommender.WebApi.TestPerformance.Statics;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MovieRecommender.WebApi.TestPerformance
{
    public class TestRunnerService : ITestRunnerService
    {
        private readonly ILogger<TestRunnerService> logger;
        private readonly HttpClient client;

        public TestRunnerService(ILogger<TestRunnerService> logger)
        {
            this.logger = logger;
            client = new HttpClient();
        }

        
        public async Task Run()
        /**
         * -------------------------------------------------------------------
         * |Average response time for a 50 request batch:        11786.2 (ms)|
         * |-----------------------------------------------------------------|
         * |Average response time for a single request:          235.724 (ms)|
         * |-----------------------------------------------------------------|
         * |Fastest response time for a batch of 50 requests:       9652 (ms)|
         * |-----------------------------------------------------------------|
         * |Slowest response time for a batch of 50 requests:      17037 (ms)|
         * -------------------------------------------------------------------
         */
        {
            logger.LogInformation("Start testing");

            for (int batchNo = 0; batchNo < Config.TotalRequests / Config.ReqBatchSize; batchNo++)
            {
                logger.LogInformation($"[BatchNo: {batchNo + 1}] Building {Config.ReqBatchSize} requests");

                var requests = PerpareRandomBatchHttpRequests(Config.ReqBatchSize);

                logger.LogInformation($"[BatchNo: {batchNo + 1}] Sending {Config.ReqBatchSize} requests");

                var watch = new Stopwatch();

                watch.Start();
                var responses = await Task.WhenAll(requests);
                watch.Stop();

                var receivedStatuses = string.Join(", ", responses.Select(r => r.StatusCode).Distinct());
                logger.LogInformation($"[BatchNo: {batchNo + 1}] Received {responses.Length} responses with the status: {receivedStatuses}. Execution time: {watch.ElapsedMilliseconds} (ms)");
            }

            logger.LogInformation("Finish testing");
        }

        private IEnumerable<Task<HttpResponseMessage>> PerpareRandomBatchHttpRequests(int batchSize)
        {
            for (int i = 0; i < batchSize; i++)
            {
                yield return PrepareRandomHttpRequest();
            }
        }

        private Task<HttpResponseMessage> PrepareRandomHttpRequest()
        {
            var query = new Dictionary<string, string>()
            {
                ["email"] = new Internet().Email()
            };

            var url = QueryHelpers.AddQueryString(Config.WebApiEndpointUnderTest, query);

            var request = new HttpRequestMessage(Config.HttpMethodUnderTest, url);

            var response = client.SendAsync(request);

            return response;
        }
    }
}
