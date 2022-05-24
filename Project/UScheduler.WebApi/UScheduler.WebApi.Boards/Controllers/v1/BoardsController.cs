using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UScheduler.WebApi.Boards.Data.Entities;
using UScheduler.WebApi.Boards.Interfaces;
using UScheduler.WebApi.Boards.Models;
using UScheduler.WebApi.Boards.Statics;

namespace UScheduler.WebApi.Boards.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BoardsController : ControllerBase
    {
        private readonly IBoardsService _provider;
        private readonly ILogger<BoardsController> _logger;

        public BoardsController(
            ILogger<BoardsController> logger,
            IBoardsService provider)
        {
            _logger = logger;
            _provider = provider;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoardById(Guid id)
        {
            _logger?.LogDebug("Handling GET request on api/v1/Boards/{id}", id);

            var (isSuccess, board, error) = await _provider.GetBoardAsync(id);
            if (isSuccess)
            {
                return Ok(board);
            }
            if (error.Equals(ErrorMessage.BoardNotFound))
            {
                return NotFound();
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = error });
        }

        [HttpGet]
        public async Task<IActionResult> GetBoardsFromWorkspace([FromQuery] Guid workspaceId)
        {
            _logger?.LogDebug("Handling GET request on api/v1/Boards?workspaceId={workspaceId}", workspaceId);

            var (isSuccess, workspace, error) = await _provider.GetBoardsByWorkspaceIdAsync(workspaceId);
            return isSuccess
                ? Ok(workspace)
                : StatusCode(StatusCodes.Status500InternalServerError, new { message = error });
        }

        [HttpPost]
        public async Task<IActionResult> CreateBoard([FromBody] CreateBoardModel model, [FromHeader] string requestedBy)
        {
            _logger?.LogDebug("Handling POST request on api/v1/Boards");

            var (isSuccess, board, error) = await _provider.CreateBoardAsync(model, requestedBy);
            return isSuccess
                ? Created(Request.Host.Value + $"/api/v1/Boards/{board.Id}", board)
                : BadRequest(new { Message = error });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoard(Guid id, [FromBody] UpdateBoardModel model, [FromHeader] string requestedBy)
        {
            _logger?.LogDebug("Handling PUT request on api/v1/Boards/{id}", id);

            var (isSuccess, board, error) = await _provider.UpdateAsync(id, model, requestedBy);
            if (isSuccess)
            {
                return Ok(board);
            }
            if (error == ErrorMessage.BoardNotFound)
            {
                return NotFound();
            }
            return BadRequest(new { Message = error });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateBoard(Guid id, [FromBody] JsonPatchDocument<Board> model, [FromHeader] string requestedBy)
        {
            _logger?.LogDebug("Handling PATCH request on api/v1/Boards/{id}", id);

            var (isSuccess, board, error) = await _provider.UpdateAsync(id, model, requestedBy);
            if (isSuccess)
            {
                return Ok(board);
            }
            if (error == ErrorMessage.BoardNotFound)
            {
                return NotFound();
            }
            return BadRequest(new { Message = error });

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard([FromRoute] Guid id)
        {
            _logger?.LogDebug("Handling DELETE request on api/v1/Boards/{id}", id);
            var (isSuccess, error) = await _provider.DeleteBoardAsync(id);
            if (isSuccess)
            {
                return NoContent();
            }
            if (error == ErrorMessage.BoardNotFound)
            {
                return NotFound();
            }
            return BadRequest(new { Message = error });
        }
    }
}
