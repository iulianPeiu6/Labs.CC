using LiteDB;
using MovieRecommender.Application.Interfaces;
using MovieRecommender.Domain.Entities;

namespace MovieRecommender.DataAccess
{
    public class RequestLogRepository : IRequestLogRepository
    {
        private readonly ILiteDatabase database;

        private readonly ILiteCollection<RequestLog> collection;

        public RequestLogRepository()
        {
            var databasePath = @"Data\MovieRecommender.db";

            var connectionString = @$"Filename={databasePath}; Connection=Shared;";

            database = new LiteDatabase(connectionString);

            collection = database.GetCollection<RequestLog>($"{ typeof(RequestLog).Name }s");
        }

        public void Add(RequestLog entity)
        {
            collection.Insert(entity);
        }

        public IEnumerable<RequestLog> GetAll()
        {
            return collection.FindAll();
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
