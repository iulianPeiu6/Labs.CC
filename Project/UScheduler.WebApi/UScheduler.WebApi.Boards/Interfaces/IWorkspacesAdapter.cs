using System;
using System.Threading.Tasks;

namespace UScheduler.WebApi.Boards.Interfaces
{
    public interface IWorkspacesAdapter
    {
        Task<(bool IsSuccess, string Error)> WorkspaceExists(Guid workspaceId);
    }
}
