using MovieRecommender.Domain.Entities;

namespace TMDb.Services.Abstracts
{
    public interface ITMDbService
    {
        Task<IList<Movie>> GetMostPopularMoviesAsync();
    }
}