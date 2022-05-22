using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UScheduler.WebApi.Tasks.Interfaces;
using UScheduler.WebApi.Tasks.Models;
using UScheduler.WebApi.Tasks.Statics;
using Task = UScheduler.WebApi.Tasks.Data.Entities.Task;

namespace UScheduler.WebApi.Tasks.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _provider;
        private readonly ILogger<TasksController> _logger;

        public TasksController(
            ITasksService provider, 
            ILogger<TasksController> logger)
        {
            _provider = provider;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskAsync([FromRoute] Guid id)
        {
            _logger?.LogDebug($"Handling GET request on api/v1/Tasks/{id}");

            var (isSuccess, task, error) = await _provider.GetTaskAsync(id);
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
            _logger?.LogDebug($"Handling GET request on api/v1/Tasks?boardId={boardId}");

            var (isSuccess, task, error) = await _provider.GetTasksByBoardIdAsync(boardId);
            return isSuccess 
                ? Ok(task) 
                : StatusCode(StatusCodes.Status500InternalServerError, new { message = error });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskModel model, [FromHeader] string requestedBy)
        {
            _logger?.LogDebug("Handling POST request on api/v1/Tasks");

            var (isSuccess, task, error) = await _provider.CreateTaskAsync(model, requestedBy);
            return isSuccess 
                ? Created(Request.Host.Value + $"/api/v1/Tasks/{task.Id}", task) 
                : BadRequest(new {Message = error});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
        {
            _logger?.LogDebug($"Handling DELETE request on api/v1/Tasks/{id}");

            var (isSuccess, error) = await _provider.DeleteTask(id);
            return isSuccess
                ? NoContent()
                : BadRequest(new {Message = error});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(
            [FromRoute] Guid id, 
            [FromBody] UpdateTaskModel model,
            [FromHeader] string requestedBy)
        {
            _logger?.LogDebug($"Handling PUT request on api/v1/Tasks/{id}");

            var (isSuccess, taskDto, error) = await _provider.UpdateTaskAsync(id, model, requestedBy);
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
            _logger?.LogDebug($"Handling PATCH request on api/v1/Tasks/{id}");

            var (isSuccess, taskDto, error) = await _provider.UpdateTaskAsync(id, model, requestedBy);
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
