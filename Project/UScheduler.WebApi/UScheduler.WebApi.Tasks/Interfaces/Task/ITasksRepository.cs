using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskEntity = UScheduler.WebApi.Tasks.Data.Entities.Task;

namespace UScheduler.WebApi.Tasks.Interfaces.Task
{
    public interface ITasksRepository
    {
        Task<TaskEntity> GetTaskAsync(Expression<Func<TaskEntity, bool>> func);
        Task<IEnumerable<TaskEntity>> GetTasksAsync(Expression<Func<TaskEntity, bool>> func);
        Task<TaskEntity> CreateTask(TaskEntity task);
        System.Threading.Tasks.Task DeleteAsync(TaskEntity task);
        System.Threading.Tasks.Task UpdateAsync(TaskEntity task);
    }
}
