using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using UScheduler.WebApi.Tasks.Models.Task;
using TaskEntity = UScheduler.WebApi.Tasks.Data.Entities.Task;

namespace UScheduler.WebApi.Tasks.Interfaces.Task
{
    public interface ITasksService
    {
        Task<(bool IsSuccess, TaskDto Task, string Error)> GetTaskAsync(Guid id);
        Task<(bool IsSuccess, IEnumerable<TaskDto> Task, string Error)> GetTasksByBoardIdAsync(Guid boardId);
        Task<(bool IsSuccess, TaskDto Task, string error)> CreateTaskAsync(CreateTaskModel model, string createdBy);
        Task<(bool IsSuccess, TaskDto taskDto, string error)> UpdateTaskAsync(Guid id, UpdateTaskModel model, string updatedBy);
        Task<(bool IsSuccess, TaskDto taskDto, string error)> UpdateTaskAsync(Guid id, JsonPatchDocument<TaskEntity> model, string updatedBy);
        Task<(bool IsSuccess, string error)> DeleteTask(Guid id);
    }
}
