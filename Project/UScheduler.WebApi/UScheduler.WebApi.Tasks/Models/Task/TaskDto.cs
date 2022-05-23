using System;
using System.Collections.Generic;
using UScheduler.WebApi.Tasks.Models.ToDo;

namespace UScheduler.WebApi.Tasks.Models.Task
{
    public class TaskDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DueDateTime { get; set; }

        public ICollection<ToDoDto> ToDoChecks { get; set; }

        public Guid BoardId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }
    }
}
