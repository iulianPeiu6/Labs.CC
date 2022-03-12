namespace MovieRecommender.WebApi.TestPerformance.Statics
{
    public static class Config
    {
        public static string WebApiEndpointUnderTest = "https://localhost:7173/api/1/Notification/SendRecommendationsViaEmail";
        public static HttpMethod HttpMethodUnderTest = HttpMethod.Post;
        public static int TotalRequests = 500;
        public static int ReqBatchSize = 50;
    }
}
