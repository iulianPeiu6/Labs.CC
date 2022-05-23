using Microsoft.EntityFrameworkCore;
using UScheduler.WebApi.Tasks.Data.Entities;

namespace UScheduler.WebApi.Tasks.Data
{
    public class TasksContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<ToDo> ToDos { get; set; }

        public TasksContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
