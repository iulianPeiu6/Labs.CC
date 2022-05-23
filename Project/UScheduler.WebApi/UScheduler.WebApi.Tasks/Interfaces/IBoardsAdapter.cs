using System;
using System.Threading.Tasks;

namespace UScheduler.WebApi.Tasks.Interfaces
{
    public interface IBoardsAdapter
    {
        Task<(bool IsSuccess, string error)> BoardExists(Guid boardId);
    }
}
