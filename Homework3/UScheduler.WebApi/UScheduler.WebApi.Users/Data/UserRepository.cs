using LiteDB;
using System.Linq.Expressions;
using UScheduler.WebApi.Users.Data.Entities;

namespace UScheduler.WebApi.Users.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ILiteDatabase database;

        private readonly ILiteCollection<User> collection;

        public UserRepository()
        {

            var databasePath = Path.Combine("Data", "Users.db");

            var connectionString = @$"Filename={databasePath}; Connection=Shared;";

            database = new LiteDatabase(connectionString);

            collection = database.GetCollection<User>($"{ typeof(User).Name }s");
        }

        public User Add(User entity)
        {
            return collection.FindById(collection.Insert(entity));
        }

        public IEnumerable<User> GetAll()
        {
            return collection.FindAll();
        }

        public User GetById(Guid id)
        {
            return collection.FindById(id);
        }

        public User Update(Guid id, User entity)
        {
            collection.Update(id, entity);

            var result = collection.FindById(id);

            if (result == null)
            {
                throw new ArgumentNullException("Entity not found");
            }

            return result;
        }

        public bool Delete(Guid id)
        {
            return collection.Delete(id);
        }

        public ILiteQueryable<User> Query(Expression<Func<User, bool>> expression)
        {
            return collection.Query().Where(expression);
        }

        public ILiteQueryable<User> Query()
        {
            return collection.Query();
        }

        public void Dispose()
        {
            database.Dispose();
        }
    }
}
