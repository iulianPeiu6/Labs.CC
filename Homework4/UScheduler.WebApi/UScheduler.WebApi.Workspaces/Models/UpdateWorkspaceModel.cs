using Microsoft.AspNetCore.Mvc;

namespace UScheduler.WebApi.Workspaces.Models
{
    public class UpdateWorkspaceModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string AccessType { get; set; }

        public string UpdatedBy { get; set; }
    }
}
