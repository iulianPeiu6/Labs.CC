namespace MovieRecommender.Domain.Entities
{
    public class Report
    {
        public IEnumerable<RequestLog> APIRequests { get; set; }

        public int NumberOfRequestsOnYoutube { get; set; }
        public int NumberOfRequestsOnTMDb { get; set; }
        public int NumberOfRequestsOnSendGrid { get; set; }

        public double MeanLatencyForYoutube { get; set; }
        public double MeanLatencyForTMDb { get; set; }
        public double MeanLatencyForSendGrid { get; set; }

        public int NumberOfNotOkResponsesOnYoutube { get; set; }
        public int NumberOfNotOkResponsesOnTMDb { get; set; }
        public int NumberOfNotOkResponsesOnSendGrid { get; set; }

        public IEnumerable<int> LatenciesForYoutube { get; set; }
        public IEnumerable<int> LatenciesForTMDb { get; set; }
        public IEnumerable<int> LatenciesForSendGrid { get; set; }
    }
}
