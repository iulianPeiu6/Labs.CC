using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskEntity = UScheduler.WebApi.Tasks.Data.Entities.Task;

namespace UScheduler.WebApi.Tasks.Interfaces
{
    public interface ITasksRepository
    {
        Task<TaskEntity> GetTaskAsync(Expression<Func<TaskEntity, bool>> func);
        Task<IEnumerable<TaskEntity>> GetTasksAsync(Expression<Func<TaskEntity, bool>> func);
        Task<TaskEntity> CreateTask(TaskEntity task);
        Task DeleteAsync(TaskEntity board);
        Task UpdateAsync(TaskEntity board);
    }
}
