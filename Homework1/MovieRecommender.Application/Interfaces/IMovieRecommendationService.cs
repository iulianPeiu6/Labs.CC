using MovieRecommender.Domain.Entities;

namespace MovieRecommender.Application.Interfaces
{
    public interface IMovieRecommendationService
    {
        Task<IList<Movie>> GetRecommendationsAsync();
        Task<bool> SendRecommendationViaMailAsync(IList<Movie> movies, string email);
    }
}