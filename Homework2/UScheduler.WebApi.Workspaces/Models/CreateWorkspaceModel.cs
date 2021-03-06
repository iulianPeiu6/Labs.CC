namespace UScheduler.WebApi.Workspaces.Models
{
    public class CreateWorkspaceModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid Owner { get; set; }

        public string AccessType { get; set; }

        public List<string> ColabUsersIds { get; set; }

        public string WorkspaceType { get; set; }
    }
}
