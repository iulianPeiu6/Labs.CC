using AutoMapper;
using UScheduler.WebApi.Workspaces.Data.Entities;
using UScheduler.WebApi.Workspaces.Models;

namespace UScheduler.WebApi.Workspaces.MappingProfiles
{
    public class WorkspacesProfile : Profile
    {
        public WorkspacesProfile()
        {
            CreateMap<Workspace, WorkspaceDto>();
            CreateMap<WorkspaceDto, Workspace>();
            CreateMap<CreateWorkspaceModel, WorkspaceDto>();
            CreateMap<CreateWorkspaceModel, Workspace>();
            CreateMap<UpdateWorkspaceModel, Workspace>();
        }
    }
}
