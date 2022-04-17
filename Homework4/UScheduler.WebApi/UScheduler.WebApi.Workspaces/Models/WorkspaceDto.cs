using System;
using System.Collections.Generic;

namespace UScheduler.WebApi.Workspaces.Models
{
    public class WorkspaceDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Owner { get; set; }

        public string AccessType { get; set; }

        public List<string> Colabs { get; set; }

        public string WorkspaceType { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
