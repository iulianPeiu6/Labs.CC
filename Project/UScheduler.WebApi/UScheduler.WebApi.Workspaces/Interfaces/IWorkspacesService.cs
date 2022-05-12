using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UScheduler.WebApi.Workspaces.Data.Entities;
using UScheduler.WebApi.Workspaces.Models;

namespace UScheduler.WebApi.Workspaces.Interfaces
{
    public interface IWorkspacesService
    {
        Task<(bool IsSuccess, WorkspaceDto Workspace, string Error)> CreateWorkspaceAsync(CreateWorkspaceModel createWorkspaceModel);
        Task<(bool IsSuccess, string Error)> DeleteWorkspaceAsync(Guid id);
        Task<(bool IsSuccess, WorkspaceDto Workspace, string Error)> FullUpdateWorkspaceAsync(Guid id, UpdateWorkspaceModel updateWorkspaceModel);
        Task<(bool IsSuccess, IEnumerable<WorkspaceDto> Workspaces, string Error)> GetOwnerWorkspacesAsync(string owner);
        Task<(bool IsSuccess, WorkspaceDto Workspace, string Error)> PartiallyUpdateWorkspaceAsync(Guid id, JsonPatchDocument<Workspace> patchDoc, string updatedBy);
    }
}