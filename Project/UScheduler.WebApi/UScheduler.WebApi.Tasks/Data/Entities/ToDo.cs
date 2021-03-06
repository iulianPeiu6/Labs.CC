using System;

namespace UScheduler.WebApi.Tasks.Data.Entities
{
    public class ToDo
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public bool Completed { get; set; }

        public Task Task { get; set; }

        public DateTime CreatedAt { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string UpdatedBy { get; set; }

    }
}
