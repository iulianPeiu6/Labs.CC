using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UScheduler.WebApi.Workspaces.Data;
using UScheduler.WebApi.Workspaces.Data.Entities;
using UScheduler.WebApi.Workspaces.Interfaces;
using UScheduler.WebApi.Workspaces.Models;
using UScheduler.WebApi.Workspaces.Statics;

namespace UScheduler.WebApi.Workspaces.Services
{
    public class WorkspacesService : IWorkspacesService
    {
        private readonly WorkspacesContext context;
        private readonly ILogger<WorkspacesService> logger;
        private readonly IMapper mapper;

        public WorkspacesService(
            WorkspacesContext context,
            ILogger<WorkspacesService> logger,
            IMapper mapper)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
        }

        public async Task<(bool IsSuccess, WorkspaceDto Workspace, string Error)> CreateWorkspaceAsync(CreateWorkspaceModel createWorkspaceModel)
        {
            try
            {
                var workspace = mapper.Map<Workspace>(createWorkspaceModel);
                var response = await context.Workspaces.AddAsync(workspace);
                workspace = response.Entity;
                await context.SaveChangesAsync();
                var result = mapper.Map<WorkspaceDto>(workspace);
                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<WorkspaceDto> Workspaces, string Error)> GetOwnerWorkspacesAsync(Guid owner)
        {
            try
            {
                var workspaces = await context.Workspaces
                    .Where(w => w.Owner == owner)
                    .ToListAsync();

                var result = mapper.Map<IEnumerable<WorkspaceDto>>(workspaces);
                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, WorkspaceDto Workspace, string Error)> FullUpdateWorkspaceAsync(Guid id, UpdateWorkspaceModel updateWorkspaceModel)
        {
            try
            {
                var workspace = await context.Workspaces
                    .SingleOrDefaultAsync(w => w.Id == id);

                if (workspace == null)
                {
                    return (false, null, ErrorMessage.WorkspaceNotFound);
                }

                workspace.Title = updateWorkspaceModel.Title;
                workspace.Description = updateWorkspaceModel.Description;
                workspace.Owner = updateWorkspaceModel.Owner;
                workspace.AccessType = updateWorkspaceModel.AccessType;
                workspace.WorkspaceType = updateWorkspaceModel.WorkspaceType;

                context.Workspaces.Update(workspace);
                await context.SaveChangesAsync();

                var result = mapper.Map<WorkspaceDto>(workspace);
                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, WorkspaceDto Workspace, string Error)> PartiallyUpdateWorkspaceAsync(Guid id, JsonPatchDocument<Workspace> patchDoc)
        {
            try
            {
                var workspace = await context.Workspaces
                    .SingleOrDefaultAsync(w => w.Id == id);

                if (workspace == null)
                {
                    return (false, null, ErrorMessage.WorkspaceNotFound);
                }

                patchDoc.ApplyTo(workspace);

                context.Workspaces.Update(workspace);
                await context.SaveChangesAsync();

                var result = mapper.Map<WorkspaceDto>(workspace);
                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string Error)> DeleteWorkspaceAsync(Guid id)
        {
            try
            {
                var workspace = await context.Workspaces
                    .SingleOrDefaultAsync(w => w.Id == id);

                if (workspace == null)
                {
                    return (false, ErrorMessage.WorkspaceNotFound);
                }

                context.Remove(workspace);
                await context.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, ex.Message);
            }
        }
    }
}
