using MovieRecommender.Application.Interfaces;
using MovieRecommender.Domain.Entities;
using SendGrid.Services.Abstracts;
using System.Text;
using TMDb.Services.Abstracts;
using Youtube.Services.Abstracts;

namespace MovieRecommender.Application.Services
{
    public class MovieRecommendationService : IMovieRecommendationService
    {
        private readonly ITMDbService tmdbService;
        private readonly IYoutubeService youtubeService;
        private readonly ISendGridService sendgridService;

        public MovieRecommendationService(
            ITMDbService tmdbService,
            IYoutubeService youtubeService,
            ISendGridService sendgridService)
        {
            this.tmdbService = tmdbService;
            this.youtubeService = youtubeService;
            this.sendgridService = sendgridService;
        }

        public async Task<IList<Movie>> GetRecommendationsAsync()
        {
            var movies = await tmdbService.GetMostPopularMoviesAsync();

            await UpdateMoviesWithYoutubeTrailerLinkAsync(movies);

            return movies;
        }

        public async Task<bool> SendRecommendationViaMailAsync(IList<Movie> movies, string email)
        {
            var mail = new Mail
            {
                ReceiverEmailAddress = email,
                Body = BuildEmailBody(movies)
            };

            return await sendgridService.SendAsync(mail);
        }

        private async Task UpdateMoviesWithYoutubeTrailerLinkAsync(IList<Movie> movies)
        {
            foreach (var movie in movies)
            {
                var searchKey = $"{movie.Title}";
                var youtubeTrailerLink = await youtubeService.GetFirstSearchVideoLinkAsync(searchKey);
                movie.YoutubeTrailerLink = youtubeTrailerLink;
            }
        }

        private string BuildEmailBody(IList<Movie> movies)
        {
            var body = new StringBuilder();
            body.Append("Hello, <br/><br/>");
            body.Append("We believe that you might like the following movies. <br/><br/>");

            body.Append("<table style=\"width: 60%;\">");

            body.Append("<tr style=\"background-color: #ffffff;>");
            body.Append("<th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">&nbsp;</th>");
            body.Append("<th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Title</th>");
            body.Append("<th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Release Date</th>");
            body.Append("<th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Rating</th>");
            body.Append("<th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">&nbsp;</th>");
            body.Append("</tr>");

            var row = 0;
            foreach (var movie in movies)
            {
                if (row % 2 == 0)
                {
                    body.Append("<tr style=\"background-color: #dddddd;\">");
                }
                else
                {
                    body.Append("<tr style=\"background-color: #ffffff;\">");
                }

                body.Append($"<td style=\"border: 1px solid #dddddd; text-align: center; padding: 8px; display:block;\">{++row}</th>");
                body.Append($"<td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{movie.Title}</th>");
                body.Append($"<td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{movie.ReleaseDate.ToString("dd/MM/yyyy")}</th>");
                body.Append($"<td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{movie.Rating.ToString("#.00#")} | {movie.NumberOfRatings}</th>");
                body.Append($"<td style=\"border: 1px solid #dddddd; text-align: center; padding: 8px;\"><a href=\"{movie.YoutubeTrailerLink}\">View Trailer</a></th>");
                body.Append("</tr>");
            }
            body.Append("</table><br/>");

            body.Append("Watchster, <br/>");
            body.Append("Your Online Movie Recommender");

            return body.ToString();
        }
    }
}
