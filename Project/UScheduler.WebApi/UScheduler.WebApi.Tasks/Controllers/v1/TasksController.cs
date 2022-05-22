using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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

        public TasksController(ITasksService provider)
        {
            _provider = provider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskAsync([FromRoute] Guid id)
        {
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
        public async Task<IActionResult> GetTasksFromBoard([FromQuery] Guid id)
        {
            var (isSuccess, task, error) = await _provider.GetTasksByBoardIdAsync(id);
            return isSuccess 
                ? Ok(task) 
                : StatusCode(StatusCodes.Status500InternalServerError, new { message = error });
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskModel model, [FromHeader] string requestedBy)
        {
            var (isSuccess, task, error) = await _provider.CreateTaskAsync(model, requestedBy);
            return isSuccess 
                ? Created(Request.Host.Value + $"/api/v1/Tasks/{task.Id}", task) 
                : BadRequest(new {Message = error});
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] Guid id)
        {
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
