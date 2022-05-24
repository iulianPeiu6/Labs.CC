using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UScheduler.WebApi.Tasks.Data;
using UScheduler.WebApi.Tasks.Interfaces.Task;
using TaskEntity = UScheduler.WebApi.Tasks.Data.Entities.Task;

namespace UScheduler.WebApi.Tasks.Repositories
{
    public class TasksRepository : ITasksRepository
    {
        private readonly TasksContext _context;

        public TasksRepository(TasksContext context)
        {
            _context = context;
        }

        public async Task<TaskEntity> GetTaskAsync(Expression<Func<TaskEntity, bool>> func) 
            => await _context.Tasks
                .Include(task => task.ToDoChecks)
                .FirstOrDefaultAsync(func);

        public async Task<IEnumerable<TaskEntity>> GetTasksAsync(Expression<Func<TaskEntity, bool>> func) 
            => await _context.Tasks
                .AsNoTracking()
                .Include(task => task.ToDoChecks)
                .Where(func)
                .ToListAsync();

        public async Task<TaskEntity> CreateTask(TaskEntity task)
        {
            var entry = await _context.Tasks.AddAsync(task);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }
        public async Task DeleteAsync(TaskEntity task)
        {
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskEntity task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
