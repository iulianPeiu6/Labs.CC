namespace MovieRecommender.Domain.Entities
{
    public class RequestLog
    {
        public Guid Id { get; set; }
        public string Provider { get; set; }
        public string HttpMethod { get; set; }
        public string Endpoint { get; set; }
        public string RequestContent { get; set; }
        public string StatusCode { get; set; }
        public string ResponseContent { get; set; }
        public int Latency { get; set; }
    }
}
