using MovieRecommender.Domain.Entities;

namespace MovieRecommender.Application.Interfaces
{
    public interface IAPIsReportingService
    {
        Report GenerateReport();
    }
}