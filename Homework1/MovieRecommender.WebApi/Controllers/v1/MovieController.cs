using Microsoft.AspNetCore.Mvc;
using MovieRecommender.Application.Interfaces;
using MovieRecommender.Domain.Entities;

namespace MovieRecommender.WebApi.Controllers.v1
{
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRecommendationService movieRecommendationService;

        public MovieController(IMovieRecommendationService movieRecommendationService)
        {
            this.movieRecommendationService = movieRecommendationService;
        }

        [HttpGet]
        [Route("GetRecommendations")]
        public async Task<IList<Movie>> GetRecommendationsAsync()
        {
            var movies = await movieRecommendationService.GetRecommendationsAsync();
            return movies;
        }
    }
}
