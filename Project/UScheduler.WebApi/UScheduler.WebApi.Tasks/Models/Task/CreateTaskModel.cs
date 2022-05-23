using System;

namespace UScheduler.WebApi.Tasks.Models.Task;

public class CreateTaskModel
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime DueDateTime { get; set; }

    public Guid BoardId { get; set; }
}