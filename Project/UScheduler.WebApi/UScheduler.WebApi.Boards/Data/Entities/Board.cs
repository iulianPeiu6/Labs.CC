using System;

namespace UScheduler.WebApi.Boards.Data.Entities
{
    public class Board
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string BoardTemplate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public Guid WorkspaceId { get; set; }
    }
}
