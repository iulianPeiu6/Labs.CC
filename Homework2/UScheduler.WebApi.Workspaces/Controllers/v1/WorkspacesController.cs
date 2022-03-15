using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UScheduler.WebApi.Workspaces.Interfaces;
using UScheduler.WebApi.Workspaces.Models;
using UScheduler.WebApi.Workspaces.Statics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UScheduler.WebApi.Workspaces.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class WorkspacesController : ControllerBase
    {
        private readonly IWorkspacesService provider;
        private readonly ILogger<WorkspacesController> logger;

        public WorkspacesController(
            IWorkspacesService provider,
            ILogger<WorkspacesController> logger)
        {
            this.provider = provider;
            this.logger = logger;
        }

        [HttpGet("GroupedByOwners/{ownerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWorkspacesByOwnerIdAsync(Guid ownerId)
        {
            logger?.LogDebug("Handeling GET request on api/v1/Workspaces/GroupedByOwners/{id}");
            var result = await provider.GetOwnerWorkspacesAsync(ownerId);

            if (result.IsSuccess)
            {
                return Ok(result.Workspaces);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = result.Error });
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateWorkspaceAsync([FromBody] CreateWorkspaceModel workspace)
        {
            logger?.LogDebug("Handeling POST request on api/v1/Workspaces/{id}");
            var result = await provider.CreateWorkspaceAsync(workspace);

            if (result.IsSuccess)
            {
                return Created(Request.Host.Value + $"/api/v1/Workspaces/{result.Workspace.Id}", result.Workspace);
            }

            return BadRequest(new { Message = result.Error });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateWorkspaceModel workspace)
        {
            logger?.LogDebug("Handeling PUT request on api/v1/Workspaces/{id}");
            var result = await provider.FullUpdateWorkspaceAsync(id, workspace);

            if (result.IsSuccess)
            {
                return Ok(result.Workspace);
            }

            if (result.Error == ErrorMessage.WorkspaceNotFound)
            {
                return NotFound(new { Message = result.Error });
            }

            return BadRequest(new { Message = result.Error });
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] JsonPatchDocument<UpdateWorkspaceModel> patchDoc)
        {
            logger?.LogDebug("Handeling PATCH request on api/v1/Workspaces/{id}");
            if (patchDoc != null)
            {
                var workspace = new UpdateWorkspaceModel();
                patchDoc.ApplyTo(workspace, ModelState);

                var result = await provider.PartiallyUpdateWorkspaceAsync(id, workspace);

                if (result.IsSuccess)
                {
                    return Ok(result.Workspace);
                }

                if (result.Error == ErrorMessage.WorkspaceNotFound)
                {
                    return NotFound(new { message = result.Error });
                }

                return BadRequest(new { message = result.Error });
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            logger?.LogDebug("Handeling DELETE request on api/v1/Workspaces/{id}");
            var result = await provider.DeleteWorkspaceAsync(id);

            if (result.IsSuccess)
            {
                return Ok();
            }

            if (result.Error == ErrorMessage.WorkspaceNotFound)
            {
                return NotFound(new { Message = result.Error });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = result.Error });
        }
    }
}
