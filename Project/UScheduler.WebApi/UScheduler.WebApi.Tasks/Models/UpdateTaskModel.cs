using System;

namespace UScheduler.WebApi.Tasks.Models
{
    public class UpdateTaskModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDateTime { get; set; }
    }
}
