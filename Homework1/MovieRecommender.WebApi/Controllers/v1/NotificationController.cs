using Microsoft.AspNetCore.Mvc;
using MovieRecommender.Application.Interfaces;

namespace MovieRecommender.WebApi.Controllers.v1
{
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class NotificationController : ControllerBase
    {
        private readonly IMovieRecommendationService movieRecommendationService;

        public NotificationController(IMovieRecommendationService movieRecommendationService)
        {
            this.movieRecommendationService = movieRecommendationService;
        }

        [HttpPost]
        [Route("SendRecommendationsViaEmail")]
        public async Task<bool> SendRecommendationsViaEmailAsync(string email)
        {
            var movies = await movieRecommendationService.GetRecommendationsAsync();
            return await movieRecommendationService.SendRecommendationViaMailAsync(movies, email);
        }
    }
}
