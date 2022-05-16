using System;

namespace UScheduler.WebApi.Boards.Models
{
    public class CreateBoardModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string BoardTemplate { get; set; }
        public string CreatedBy { get; set; }
        public Guid WorkspaceId { get; set; }
    }
}
