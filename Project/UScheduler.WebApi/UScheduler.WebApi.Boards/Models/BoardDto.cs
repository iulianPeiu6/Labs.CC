using System;

namespace UScheduler.WebApi.Boards.Models
{
    public class BoardDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BoardTemplate { get; set; }
        public Guid WorkspaceId { get; set; }
    }
}
