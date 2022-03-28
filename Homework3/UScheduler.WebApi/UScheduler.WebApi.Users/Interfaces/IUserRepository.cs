using LiteDB;
using System.Linq.Expressions;
using UScheduler.WebApi.Users.Data.Entities;

namespace UScheduler.WebApi.Users.Data
{
    public interface IUserRepository
    {
        User Add(User entity);
        bool Delete(Guid id);
        void Dispose();
        IEnumerable<User> GetAll();
        User GetById(Guid id);
        ILiteQueryable<User> Query();
        ILiteQueryable<User> Query(Expression<Func<User, bool>> expression);
        User Update(Guid id, User entity);
    }
}