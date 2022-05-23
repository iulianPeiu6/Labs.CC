using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UScheduler.WebApi.Tasks.Data;
using UScheduler.WebApi.Tasks.Data.Entities;

namespace UScheduler.WebApi.Tasks.IntegrationTests
{
    public class IntegrationTestBase
    {
        private static IntegrationTestBase instance = new();
        protected readonly HttpClient testClient;
        protected readonly Guid existentTaskId = Guid.Parse("7c139c88-48d6-4233-8584-4db3389cf3e1");
        protected static DateTime dueDateTime = DateTime.UtcNow.AddDays(5);
        protected static DateTime currentDateTime = DateTime.UtcNow;
        protected IntegrationTestBase()
        {
            if (instance is null)
            {
                var appFactory = new WebApplicationFactory<Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureServices(services =>
                        {
                            services.RemoveAll(typeof(DbContextOptions<TasksContext>));
                            services.AddDbContext<TasksContext>(options =>
                            {
                                options.UseInMemoryDatabase("BoardsTestDb");
                            });
                        });
                    });

                var scopeFactory = appFactory.Server.Services.GetService<IServiceScopeFactory>();
                using (var scope = scopeFactory?.CreateScope())
                {
                    var context = scope?.ServiceProvider?.GetRequiredService<TasksContext>();
                    if (context == null)
                    {
                        throw new ArgumentNullException(nameof(context));
                    }
                    SeedRecordsInDatabase(context);
                }
                testClient = appFactory.CreateClient();
                instance = this;
            }
            else
            {
                testClient = instance.testClient;
            }
        }

        private static void SeedRecordsInDatabase(TasksContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Database.EnsureCreated();

            context.Tasks.Add(new Task
            {
                Id = Guid.Parse("7c139c88-48d6-4233-8584-4db3389cf3e1"),
                BoardId = Guid.Parse("43ae9026-6e1a-4d90-ae99-92d810e998c7"),
                Description = "Task description - 001",
                Title = "Task title - 001",
                DueDateTime = dueDateTime,
                CreatedAt = currentDateTime,
                CreatedBy = "owner-001@email.com",
                UpdatedAt = currentDateTime,
                UpdatedBy = "owner-001@email.com"
            });
            context.Tasks.Add(new Task
            {
                Id = Guid.Parse("96bf092b-c806-4531-8579-d976d024e701"),
                BoardId = Guid.Parse("43ae9026-6e1a-4d90-ae99-92d810e998c7"),
                Description = "Task description - 002",
                Title = "Task title - 002",
                DueDateTime = dueDateTime,
                CreatedAt = currentDateTime,
                CreatedBy = "owner-002@email.com",
                UpdatedAt = currentDateTime,
                UpdatedBy = "owner-002@email.com"
            });
            context.Tasks.Add(new Task
            {
                Id = Guid.Parse("ce361462-cdb4-43c2-be2e-52003d5a4aae"),
                BoardId = Guid.Parse("c3fa7ab7-f8e7-450d-94e6-86510512b546"),
                Description = "Task description - 003",
                Title = "Task title - 003",
                DueDateTime = dueDateTime,
                CreatedAt = currentDateTime,
                CreatedBy = "owner-001@email.com",
                UpdatedAt = currentDateTime,
                UpdatedBy = "owner-001@email.com"
            });

            context.SaveChanges();
        }
    }
}
