using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UScheduler.WebApi.Tasks.Interfaces.Task;
using UScheduler.WebApi.Tasks.Interfaces.ToDo;
using UScheduler.WebApi.Tasks.Models.Task;
using UScheduler.WebApi.Tasks.Models.ToDo;
using UScheduler.WebApi.Tasks.Statics;
using Task = UScheduler.WebApi.Tasks.Data.Entities.Task;

namespace UScheduler.WebApi.Tasks.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;
        private readonly IToDoService _toDoService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(
            ITasksService tasksService, 
            ILogger<TasksController> logger, 
            IToDoService toDoService)
        {
            _tasksService = tasksService;
            _logger = logger;
            _toDoService = toDoService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskAsync([FromRoute] Guid id)
        {
            _logger?.LogDebug("Handling GET request on api/v1/Tasks/{id}", id);

            var (isSuccess, task, error) = await _tasksService.GetTaskAsync(id);
            if (isSuccess)
            {
                return Ok(task);
            }

            if (error == ErrorMessage.TaskNotFound)
            {
                return NotFound();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error });
        }

        [HttpGet]
        public async Task<IActionResult> GetTasksFromBoard([FromQuery] Guid boardId)
        {
            _logger?.LogDebug("Handling GET request on api/v1/Tasks?boardId={boardId}", boardId);

            var (isSuccess, task, error) = await _tasksService.GetTasksByBoardIdAsync(boardId);
            return isSuccess 
                ? Ok(task) 
                : StatusCode(StatusCodes.Status500InternalServerError, new { message = error });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskModel model, [FromHeader] string requestedBy)
        {
            _logger?.LogDebug("Handling POST request on api/v1/Tasks");

            var (isSuccess, task, error) = await _tasksService.CreateTaskAsync(model, requestedBy);
            return isSuccess 
                ? Created(Request.Host.Value + $"/api/v1/Tasks/{task.Id}", task) 
                : BadRequest(new {Message = error});
        }

        [HttpPost("{taskId}/ToDos")]
        public async Task<IActionResult> AddTodo([FromRoute] Guid taskId, [FromBody] CreateToDoModel model, [FromHeader] string requestedBy)
        {
            var (isSuccess, toDoDto, error) = await _toDoService.CreateToDoAsync(taskId, model, requestedBy);
            if (isSuccess)
            {
                return Created("", toDoDto);
            }

            if (error == ErrorMessage.TaskNotFound)
            {
                return NotFound(new {Message = error});
            }

            return BadRequest(new { Message = error });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
        {
            _logger?.LogDebug("Handling DELETE request on api/v1/Tasks/{id}", id);

            var (isSuccess, error) = await _tasksService.DeleteTask(id);
            if (isSuccess)
                return NoContent();
            if (error == ErrorMessage.TaskNotFound)
            {
                return NotFound();
            }
            return BadRequest(new {Message = error});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(
            [FromRoute] Guid id, 
            [FromBody] UpdateTaskModel model,
            [FromHeader] string requestedBy)
        {
            _logger?.LogDebug("Handling PUT request on api/v1/Tasks/{id}", id);

            var (isSuccess, taskDto, error) = await _tasksService.UpdateTaskAsync(id, model, requestedBy);
            if (isSuccess)
            {
                return Ok(taskDto);
            }

            if (error == ErrorMessage.TaskNotFound)
            {
                return NotFound();
            }
            return BadRequest(new { Message = error });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateTask(
            [FromRoute] Guid id,
            [FromBody] JsonPatchDocument<Task> model,
            [FromHeader] string requestedBy)
        {
            _logger?.LogDebug("Handling PATCH request on api/v1/Tasks/{id}", id);

            var (isSuccess, taskDto, error) = await _tasksService.UpdateTaskAsync(id, model, requestedBy);
            if (isSuccess)
            {
                return Ok(taskDto);
            }

            if (error == ErrorMessage.TaskNotFound)
            {
                return NotFound();
            }
            return BadRequest(new { Message = error });
        }
    }
}
