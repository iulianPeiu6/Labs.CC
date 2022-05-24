using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using UScheduler.WebApi.Tasks.Data.Entities;
using UScheduler.WebApi.Tasks.Interfaces.Task;
using UScheduler.WebApi.Tasks.Interfaces.ToDo;
using UScheduler.WebApi.Tasks.Models.ToDo;
using UScheduler.WebApi.Tasks.Statics;

namespace UScheduler.WebApi.Tasks.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IToDoRepository _toDoRepository;
        private readonly ITasksRepository _tasksRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ToDoService> _logger;

        public ToDoService(
            IToDoRepository toDoRepository,
            ITasksRepository tasksRepository,
            IMapper mapper, 
            ILogger<ToDoService> logger)
        {
            _toDoRepository = toDoRepository;
            _mapper = mapper;
            _logger = logger;
            _tasksRepository = tasksRepository;
        }

        public async Task<(bool IsSuccess, ToDoDto toDoDto, string error)> GetToDoAsync(Guid id)
        {
            try
            {
                var todo = await _toDoRepository.GetToDo(todo => todo.Id == id);
                if (todo == null)
                {
                    return (false, null, ErrorMessage.ToDoNotFound);
                }

                var toDoDto = _mapper.Map<ToDoDto>(todo);
                return (true, toDoDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<ToDoDto> toDoDtos, string error)> GetToDosAsync(Guid taskId)
        {
            try
            {
                var toDos = await _toDoRepository.GetToDos(todo => todo.Task.Id == taskId);

                var toDoDto = _mapper.Map<IEnumerable<ToDoDto>>(toDos);
                return (true, toDoDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, ToDoDto toDoDto, string error)> CreateToDoAsync(Guid taskId, CreateToDoModel model, string requestedBy)
        {
            try
            {
                var task = await _tasksRepository.GetTaskAsync(task => task.Id == taskId);
                if (task == null)
                {
                    return (false, null, ErrorMessage.TaskNotFound);
                }

                var todo = _mapper.Map<ToDo>(model);
                var currentTime = DateTime.UtcNow;

                todo.CreatedAt = currentTime;
                todo.UpdatedAt = currentTime;
                todo.CreatedBy = requestedBy;
                todo.UpdatedBy = requestedBy;
                todo.Task = task;

                var createdDto = await _toDoRepository.CreateTodo(todo);
                var toDoDto = _mapper.Map<ToDoDto>(createdDto);
                return (true, toDoDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, ToDoDto toDoDto, string error)> UpdateToDoAsync(Guid id, UpdateToDoModel model, string requestedBy)
        {
            try
            {
                var todo = await _toDoRepository.GetToDo(todo => todo.Id == id);
                if (todo == null)
                {
                    return (false, null, ErrorMessage.ToDoNotFound);
                }

                todo.UpdatedAt = DateTime.UtcNow;
                todo.UpdatedBy = requestedBy;
                todo.Completed = model.Completed;
                todo.Description = model.Description;
                await _toDoRepository.UpdateTodo(todo);

                var toDoDto = _mapper.Map<ToDoDto>(todo);
                return (true, toDoDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, ToDoDto toDoDto, string error)> UpdateToDoAsync(Guid id, JsonPatchDocument<ToDo> model, string requestedBy)
        {
            try
            {
                var todo = await _toDoRepository.GetToDo(todo => todo.Id == id);
                if (todo == null)
                {
                    return (false, null, ErrorMessage.ToDoNotFound);
                }

                model.ApplyTo(todo);
                todo.UpdatedAt = DateTime.UtcNow;
                todo.UpdatedBy = requestedBy;
                await _toDoRepository.UpdateTodo(todo);

                var toDoDto = _mapper.Map<ToDoDto>(todo);
                return (true, toDoDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string error)> DeleteToDoAsync(Guid id)
        {
            try
            {
                var todo = await _toDoRepository.GetToDo(todo => todo.Id == id);
                if (todo == null)
                {
                    return (false, ErrorMessage.ToDoNotFound);
                }
                await _toDoRepository.DeleteToDo(todo);
                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError("{ex}", ex);
                return (false, ex.Message);
            }
        }
    }
}
