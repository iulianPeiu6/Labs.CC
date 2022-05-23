using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using UScheduler.WebApi.Tasks.Models.ToDo;

namespace UScheduler.WebApi.Tasks.Interfaces.ToDo
{
    public interface IToDoService
    {
        Task<(bool IsSuccess, ToDoDto toDoDto, string error)> GetToDoAsync(Guid id);
        Task<(bool IsSuccess, IEnumerable<ToDoDto> toDoDtos, string error)> GetToDosAsync(Guid taskId);
        Task<(bool IsSuccess, ToDoDto toDoDto, string error)> CreateToDoAsync(Guid taskId, CreateToDoModel model, string requestedBy);
        Task<(bool IsSuccess, ToDoDto toDoDto, string error)> UpdateToDoAsync(
            Guid id, 
            UpdateToDoModel model,
            string requestedBy);
        Task<(bool IsSuccess, ToDoDto toDoDto, string error)> UpdateToDoAsync(
            Guid id,
            JsonPatchDocument<Data.Entities.ToDo> model,
            string requestedBy);
        Task<(bool IsSuccess, string error)> DeleteToDoAsync(Guid id);
    }
}
