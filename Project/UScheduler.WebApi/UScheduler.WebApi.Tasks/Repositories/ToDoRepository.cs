using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UScheduler.WebApi.Tasks.Data;
using UScheduler.WebApi.Tasks.Data.Entities;
using UScheduler.WebApi.Tasks.Interfaces.ToDo;
using Task = System.Threading.Tasks.Task;

namespace UScheduler.WebApi.Tasks.Repositories
{
    public class ToDoRepository : IToDoRepository
    {
        private readonly TasksContext _context;

        public ToDoRepository(TasksContext context)
        {
            _context = context;
        }

        public async Task<ToDo> GetToDo(Expression<Func<ToDo, bool>> func) 
            => await _context.ToDos.FirstOrDefaultAsync(func);

        public async Task<IEnumerable<ToDo>> GetToDos(Expression<Func<ToDo, bool>> func) 
            => await _context.ToDos.AsNoTracking().Where(func).ToListAsync();

        public async Task<ToDo> CreateTodo(ToDo toDo)
        {
            var entity = await _context.ToDos.AddAsync(toDo);
            await _context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task UpdateTodo(ToDo toDo)
        {
            _context.ToDos.Update(toDo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteToDo(ToDo toDo)
        {
            _context.ToDos.Remove(toDo);
            await _context.SaveChangesAsync();
        }
    }
}
