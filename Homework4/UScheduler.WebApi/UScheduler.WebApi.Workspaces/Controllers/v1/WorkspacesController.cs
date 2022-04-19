using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using UScheduler.Common.SecretsManager;
using UScheduler.WebApi.Workspaces.Data.Entities;
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

        [HttpGet]
        public async Task<IActionResult> GetWorkspacesByOwnerAsync([FromQuery] string owner)
        {
            logger?.LogDebug($"Handeling GET request on api/v1/Workspaces?owner={owner}");
            var result = await provider.GetOwnerWorkspacesAsync(owner);

            if (result.IsSuccess)
            {
                return Ok(result.Workspaces);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = result.Error });
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkspaceAsync([FromBody] CreateWorkspaceModel workspace, [FromHeader] string createdBy)
        {
            logger?.LogDebug("Handeling POST request on api/v1/Workspaces");

            workspace.CreatedBy = createdBy;

            var result = await provider.CreateWorkspaceAsync(workspace);

            if (result.IsSuccess)
            {
                return Created(Request.Host.Value + $"/api/v1/Workspaces/{result.Workspace.Id}", result.Workspace);
            }

            return BadRequest(new { Message = result.Error });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateWorkspaceModel workspace, [FromHeader] string updatedBy)
        {
            logger?.LogDebug($"Handeling PUT request on api/v1/Workspaces/{id}");

            workspace.UpdatedBy = updatedBy;

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
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] JsonPatchDocument<Workspace> patchDoc)
        {
            logger?.LogDebug($"Handeling PATCH request on api/v1/Workspaces/{id}");

            if (patchDoc != null)
            {
                Request.Headers.TryGetValue("UpdatedBy", out var updatedBy);
                if (string.IsNullOrEmpty(updatedBy))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = ErrorMessage.UpdatedByHeaderValueIsMissing });
                }
                var result = await provider.PartiallyUpdateWorkspaceAsync(id, patchDoc, updatedBy);

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

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            logger?.LogDebug($"Handeling DELETE request on api/v1/Workspaces/{id}");
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
