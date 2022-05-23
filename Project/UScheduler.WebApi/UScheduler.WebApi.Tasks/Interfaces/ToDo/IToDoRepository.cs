using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UScheduler.WebApi.Tasks.Interfaces.ToDo
{
    public interface IToDoRepository
    {
        Task<Data.Entities.ToDo> GetToDo(Expression<Func<Data.Entities.ToDo, bool>> func);
        Task<IEnumerable<Data.Entities.ToDo>> GetToDos(Expression<Func<Data.Entities.ToDo, bool>> func);
        Task<Data.Entities.ToDo> CreateTodo(Data.Entities.ToDo toDo);
        System.Threading.Tasks.Task UpdateTodo(Data.Entities.ToDo toDo);
        System.Threading.Tasks.Task DeleteToDo(Data.Entities.ToDo toDo);
    }
}
