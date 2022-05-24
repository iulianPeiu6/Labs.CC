using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RichardSzalay.MockHttp;
using UScheduler.WebApi.Tasks.Adapters;
using UScheduler.WebApi.Tasks.Data;
using UScheduler.WebApi.Tasks.Data.Entities;
using UScheduler.WebApi.Tasks.Interfaces;

namespace UScheduler.WebApi.Tasks.IntegrationTests
{
    public class IntegrationTestBase
    {
        private static IntegrationTestBase instance = new();
        protected readonly HttpClient testClient;
        protected readonly Guid existentTaskId = Guid.Parse("7c139c88-48d6-4233-8584-4db3389cf3e1");
        protected static readonly DateTime dueDateTime = DateTime.UtcNow.AddDays(5);
        protected static readonly DateTime currentDateTime = DateTime.UtcNow;
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
                                options.UseInMemoryDatabase("TasksTestDb");
                            });

                            services.RemoveAll(typeof(HttpClient));
                            var mockHttp = new MockHttpMessageHandler();

                            mockHttp.When("https://localhost:7115/api/v1/Boards/0b7b68dc-a4c4-437f-abf7-6e00ad61ebd3")
                                .Respond(HttpStatusCode.BadRequest);

                            var client = new HttpClient(mockHttp);
                            services.AddHttpClient<IBoardsAdapter, BoardsAdapter>();
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

            var task = new Task
            {
                Id = Guid.Parse("5939ca2e-e9de-4d13-ab6a-81461422cd1b"),
                BoardId = Guid.Parse("c3fa7ab7-f8e7-450d-94e6-86510512b546"),
                Description = "Task description - 003",
                Title = "Task title - 003",
                DueDateTime = dueDateTime,
                CreatedAt = currentDateTime,
                CreatedBy = "owner-001@email.com",
                UpdatedAt = currentDateTime,
                UpdatedBy = "owner-001@email.com"
            };
            context.Tasks.Add(task);

            context.ToDos.Add(new ToDo
            {
                Id = Guid.Parse("58caaac9-84f4-4bd4-9c91-9c22bfb47a1a"),
                Task = task,
                Description = "ToDo description - 001",
                CreatedAt = currentDateTime,
                CreatedBy = "owner-001@email.com",
                UpdatedAt = currentDateTime,
                UpdatedBy = "owner-001@email.com"

            });

            context.ToDos.Add(new ToDo
            {
                Id = Guid.Parse("c073dc23-8fe1-4751-9dd7-5dadcc76f8c8"),
                Task = task,
                Description = "ToDo description - 002",
                CreatedAt = currentDateTime,
                CreatedBy = "owner-001@email.com",
                UpdatedAt = currentDateTime,
                UpdatedBy = "owner-001@email.com"

            });

            context.ToDos.Add(new ToDo
            {
                Id = Guid.Parse("33103fa3-87ba-475c-987d-92edbfcae3aa"),
                Task = task,
                Description = "ToDo description - 003",
                CreatedAt = currentDateTime,
                CreatedBy = "owner-001@email.com",
                UpdatedAt = currentDateTime,
                UpdatedBy = "owner-001@email.com"

            });

            context.ToDos.Add(new ToDo
            {
                Id = Guid.Parse("c24563c5-7b21-4f04-b807-fb368673dc33"),
                Task = task,
                Description = "ToDo description - 004",
                CreatedAt = currentDateTime,
                CreatedBy = "owner-002@email.com",
                UpdatedAt = currentDateTime,
                UpdatedBy = "owner-002@email.com"

            });

            context.SaveChanges();
        }
    }
}
