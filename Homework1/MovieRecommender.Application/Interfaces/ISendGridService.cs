using MovieRecommender.Domain.Entities;

namespace SendGrid.Services.Abstracts
{
    public interface ISendGridService
    {
        Task<bool> SendAsync(Mail mail);
    }
}