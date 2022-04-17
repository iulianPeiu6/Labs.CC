using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace UScheduler.WebApi.Workspaces.Models
{
    public class CreateWorkspaceModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Owner { get; set; }

        public string AccessType { get; set; }

        public List<string> Colabs { get; set; }

        public string WorkspaceType { get; set; }

        public string CreatedBy { get; set; }
    }
}
