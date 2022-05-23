﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using UScheduler.WebApi.Tasks.Data.Entities;
using UScheduler.WebApi.Tasks.Interfaces;
using UScheduler.WebApi.Tasks.Interfaces.Task;
using UScheduler.WebApi.Tasks.Models;
using UScheduler.WebApi.Tasks.Models.Task;
using UScheduler.WebApi.Tasks.Models.ToDo;
using UScheduler.WebApi.Tasks.Statics;
using Task = UScheduler.WebApi.Tasks.Data.Entities.Task;

namespace UScheduler.WebApi.Tasks.Services
{
    public class TasksService : ITasksService
    {
        private readonly ITasksRepository _repository;
        private readonly IBoardsAdapter _boardsAdapter;
        private readonly IMapper _mapper;
        private readonly ILogger<TasksService> _logger;

        public TasksService(
            ITasksRepository repository,
            IBoardsAdapter boardsAdapter,
            IMapper mapper, 
            ILogger<TasksService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _boardsAdapter = boardsAdapter;
        }

        public async Task<(bool IsSuccess, TaskDto Task, string Error)> GetTaskAsync(Guid id)
        {
            try
            {
                var task = await _repository.GetTaskAsync(task => task.Id == id);

                if (task == null)
                {
                    return (false, null, ErrorMessage.TaskNotFound);
                }

                var taskDto = _mapper.Map<TaskDto>(task);
                return (true, taskDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<TaskDto> Task, string Error)> GetTasksByBoardIdAsync(Guid boardId)
        {
            try
            {
                var tasks = await _repository.GetTasksAsync(task => task.BoardId == boardId);

                var tasksDto = _mapper.Map<IEnumerable<TaskDto>>(tasks);
                return (true, tasksDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, TaskDto Task, string error)> CreateTaskAsync(CreateTaskModel model, string createdBy)
        {
            try
            {
                var (isSuccess, error) = await _boardsAdapter.BoardExists(model.BoardId);
                if (!isSuccess)
                {
                    return (false, null, error);
                }

                var task = _mapper.Map<Task>(model);
                var currentTime = DateTime.UtcNow;

                task.CreatedAt = currentTime;
                task.UpdatedAt = currentTime;
                task.CreatedBy = createdBy;
                task.UpdatedBy = createdBy;

                var createdTask = await _repository.CreateTask(task);
                var taskDto = _mapper.Map<TaskDto>(createdTask);

                return (true, taskDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, TaskDto taskDto, string error)> UpdateTaskAsync(Guid id, UpdateTaskModel model, string modifiedBy)
        {
            try
            {
                var task = await _repository.GetTaskAsync(task => task.Id == id);
                if (task == null)
                {
                    return (false, null, ErrorMessage.BoardNotFound);
                }

                task.UpdatedBy = modifiedBy;
                task.UpdatedAt = DateTime.UtcNow;
                task.Title = model.Title;
                task.Description = model.Description;
                task.DueDateTime = model.DueDateTime;

                await _repository.UpdateAsync(task);
                var taskDto = _mapper.Map<TaskDto>(task);
                return (true, taskDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, ToDoDto toDoDto, string error)> UpdateTaskAsync(Guid id, CreateToDoModel model, string updatedBy)
        {
            try
            {
                var task = await _repository.GetTaskAsync(task => task.Id == id);
                if (task == null)
                {
                    return (false, null, ErrorMessage.TaskNotFound);
                }

                var toDo = _mapper.Map<ToDo>(model);
                var currentTime = DateTime.UtcNow;
                toDo.Id = Guid.NewGuid();
                toDo.CreatedAt = currentTime;
                toDo.UpdatedAt = currentTime;
                toDo.CreatedBy = updatedBy;
                toDo.UpdatedBy = updatedBy;
                toDo.Task = task;
                toDo.Completed = false;

                task.ToDoChecks.Add(toDo);
                await _repository.UpdateAsync(task);

                var tdoDto = _mapper.Map<ToDoDto>(toDo);
                return (true, tdoDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, TaskDto taskDto, string error)> UpdateTaskAsync(Guid id, JsonPatchDocument<Task> model, string modifiedBy)
        {
            try
            {
                var task = await _repository.GetTaskAsync(task => task.Id == id);
                if (task == null)
                {
                    return (false, null, ErrorMessage.BoardNotFound);
                }

                model.ApplyTo(task);

                task.UpdatedBy = modifiedBy;
                task.UpdatedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(task);
                var taskDto = _mapper.Map<TaskDto>(task);
                return (true, taskDto, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string error)> DeleteTask(Guid id)
        {
            try
            {
                var task = await _repository.GetTaskAsync(task => task.Id == id);
                if (task == null)
                {
                    return (false, ErrorMessage.TaskNotFound);
                }

                await _repository.DeleteAsync(task);
                return (true, null);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex}");
                return (false, ex.Message);
            }
        }
    }
}
