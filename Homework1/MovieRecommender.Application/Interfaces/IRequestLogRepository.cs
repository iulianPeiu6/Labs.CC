using MovieRecommender.Domain.Entities;

namespace MovieRecommender.Application.Interfaces
{
    public interface IRequestLogRepository
    {
        void Add(RequestLog entity);
        void Dispose();
        IEnumerable<RequestLog> GetAll();
    }
}