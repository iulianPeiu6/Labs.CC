namespace MovieRecommender.Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Overview { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterUrl { get; set; }
        public decimal Rating { get; set; }
        public int NumberOfRatings { get; set; }
        public decimal Popularity { get; set; }
        public string YoutubeTrailerLink { get; set; }
    }

}
