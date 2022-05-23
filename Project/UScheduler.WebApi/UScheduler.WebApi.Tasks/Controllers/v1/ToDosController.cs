using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UScheduler.WebApi.Tasks.Data.Entities;
using UScheduler.WebApi.Tasks.Interfaces.ToDo;
using UScheduler.WebApi.Tasks.Models.ToDo;
using UScheduler.WebApi.Tasks.Statics;

namespace UScheduler.WebApi.Tasks.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ToDosController : ControllerBase
    {
        private readonly IToDoService _provider;

        public ToDosController(IToDoService provider)
        {
            _provider = provider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDo([FromRoute] Guid id)
        {
            var (isSuccess, toDoDto, error) = await _provider.GetToDoAsync(id);
            if (isSuccess)
            {
                return Ok(toDoDto);
            }

            if (error == ErrorMessage.ToDoNotFound)
            {
                return NotFound();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error });
        }

        [HttpGet]
        public async Task<IActionResult> GetToDos([FromQuery] Guid taskId)
        {
            var (isSuccess, toDoDto, error) = await _provider.GetToDosAsync(taskId);
            return isSuccess 
                ? Ok(toDoDto) 
                : StatusCode(StatusCodes.Status500InternalServerError, new { message = error });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDo([FromRoute] Guid id)
        {
            var (isSuccess, error) = await _provider.DeleteToDoAsync(id);
            if (isSuccess)
            {
                return NoContent();
            }

            if (error == ErrorMessage.ToDoNotFound)
            {
                return NotFound();
            }

            return BadRequest(new {Message = error});
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDo([FromRoute] Guid id, [FromBody] UpdateToDoModel model, [FromHeader] string requestedBy)
        {
            var (isSuccess, toDoDto, error) = await _provider.UpdateToDoAsync(id, model, requestedBy);
            if (isSuccess)
            {
                return Ok(toDoDto);
            }

            if (error == ErrorMessage.ToDoNotFound)
            {
                return NotFound();
            }

            return BadRequest(new { Message = error });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateToDo([FromRoute] Guid id, [FromBody] JsonPatchDocument<ToDo> model, [FromHeader] string requestedBy)
        {
            var (isSuccess, toDoDto, error) = await _provider.UpdateToDoAsync(id, model, requestedBy);
            if (isSuccess)
            {
                return Ok(toDoDto);
            }

            if (error == ErrorMessage.ToDoNotFound)
            {
                return NotFound();
            }

            return BadRequest(new { Message = error });
        }
    }
}
