using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using UScheduler.WebApi.Workspaces;
using UScheduler.WebApi.Workspaces.Data;
using UScheduler.WebApi.Workspaces.Data.Entities;

namespace UScheduler.WebApi.Users.IntegrationTests
{
    public class IntegrationTestBase
    {
        private static IntegrationTestBase instance = new IntegrationTestBase();
        protected readonly HttpClient testClient;
        protected readonly Guid inexistentWorkspceId = Guid.Parse("5c675d5d-9c94-48cd-8c35-bfaaae334691");

        protected IntegrationTestBase()
        {
            if (instance is null)
            {
                var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<WorkspacesContext>));
                        services.AddDbContext<WorkspacesContext>(options =>
                        {
                            options.UseInMemoryDatabase($"WorkspacesTestDb");
                        });
                    });
                });

                var scopeFactory = appFactory.Server.Services.GetService<IServiceScopeFactory>();
                using (var scope = scopeFactory?.CreateScope())
                {
                    var context = scope?.ServiceProvider?.GetRequiredService<WorkspacesContext>();
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

        private static void SeedRecordsInDatabase(WorkspacesContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Database.EnsureCreated();

            context.Add(new Workspace
            {
                Id = Guid.Parse("c176b60a-62d1-423a-bbfd-82d588800e10"),
                Owner = Guid.Parse("e27387c0-d043-4a74-861c-f9a274774974"),
                Title = "Workspace - 001",
                Description = "Workspace - Base - 001",
                AccessType = "Private",
                ColabUsersIds = new List<string>()
                {
                    "e27387c0-d043-4a74-861c-f9a274774974"
                },
                WorkspaceType = "Basic"
            });

            context.Add(new Workspace
            {
                Id = Guid.Parse("367c9423-0d7a-49d5-8376-5619804271bf"),
                Owner = Guid.Parse("e27387c0-d043-4a74-861c-f9a274774974"),
                Title = "Workspace - 002",
                Description = "Workspace - Update - 002",
                AccessType = "Private",
                ColabUsersIds = new List<string>()
                {
                    "e27387c0-d043-4a74-861c-f9a274774974"
                },
                WorkspaceType = "Basic"
            });

            context.Add(new Workspace
            {
                Id = Guid.Parse("0abf0eb1-072c-44b0-a775-7141d9d852c8"),
                Owner = Guid.Parse("e27387c0-d043-4a74-861c-f9a274774974"),
                Title = "Workspace - 003",
                Description = "Workspace - Delete - 003",
                AccessType = "Private",
                ColabUsersIds = new List<string>()
                {
                    "e27387c0-d043-4a74-861c-f9a274774974"
                },
                WorkspaceType = "Basic"
            });

            context.Add(new Workspace
            {
                Id = Guid.Parse("b620fba6-d1c7-4a7d-8a89-e52bcca5e3ad"),
                Owner = Guid.Parse("1580f64f-a6ae-4f1a-a3bb-7a2a433aa379"),
                Title = "Workspace - 004",
                Description = "Workspace - 004",
                AccessType = "Private",
                ColabUsersIds = new List<string>()
                {
                    "1580f64f-a6ae-4f1a-a3bb-7a2a433aa379"
                },
                WorkspaceType = "Basic"
            });

            context.SaveChanges();
        }
    }
}
