using System;
using System.Collections.Generic;

namespace UScheduler.WebApi.Tasks.Data.Entities
{
    public class Task
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDateTime { get; set; }

        public ICollection<ToDo> ToDoChecks { get; set; } = new List<ToDo>();

        public Guid BoardId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
