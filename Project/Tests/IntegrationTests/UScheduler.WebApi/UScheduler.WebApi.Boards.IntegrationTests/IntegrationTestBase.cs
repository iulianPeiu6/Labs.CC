using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using UScheduler.WebApi.Boards.Data;
using UScheduler.WebApi.Boards.Data.Entities;

namespace UScheduler.WebApi.Boards.IntegrationTests
{
    public class IntegrationTestBase
    {
        private static IntegrationTestBase instance = new IntegrationTestBase();
        protected readonly HttpClient testClient;
        protected readonly Guid existentBoardId = Guid.Parse("5c675d5d-9c94-48cd-8c35-bfaaae334691");
        protected IntegrationTestBase()
        {
            if (instance is null)
            {
                var appFactory = new WebApplicationFactory<Startup>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureServices(services =>
                        {
                            services.RemoveAll(typeof(DbContextOptions<BoardsContext>));
                            services.AddDbContext<BoardsContext>(options =>
                            {
                                options.UseInMemoryDatabase("BoardsTestDb");
                            });
                        });
                    });

                var scopeFactory = appFactory.Server.Services.GetService<IServiceScopeFactory>();
                using (var scope = scopeFactory?.CreateScope())
                {
                    var context = scope?.ServiceProvider?.GetRequiredService<BoardsContext>();
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

        private static void SeedRecordsInDatabase(BoardsContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Database.EnsureCreated();

            context.Add(new Board
            {
                Id = Guid.Parse("5c675d5d-9c94-48cd-8c35-bfaaae334691"),
                WorkspaceId = Guid.Parse("e6243037-8661-497b-af8f-4a9e965e8926"),
                Title = "Board - 001",
                Description = "Board - Base - 001",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "owner-001@email.com",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "owner-001@email.com"
            });

            context.Add(new Board
            {
                Id = Guid.Parse("63c56e16-6a65-411b-9899-647c40b7bf0e"),
                WorkspaceId = Guid.Parse("e6243037-8661-497b-af8f-4a9e965e8926"),
                Title = "Board - 002",
                Description = "Board - Base - 002",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "owner-001@email.com",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "owner-001@email.com"
            });

            context.Add(new Board
            {
                Id = Guid.Parse("5036f271-f9d7-438c-8014-2234be010fcc"),
                WorkspaceId = Guid.Parse("e6243037-8661-497b-af8f-4a9e965e8926"),
                Title = "Board - 003",
                Description = "Board - Base - 003",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "owner-001@email.com",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "owner-001@email.com"
            });

            context.Add(new Board
            {
                Id = Guid.Parse("35424c9e-f9ba-4c7d-b5fd-8f0367b75b9b"),
                WorkspaceId = Guid.Parse("43ae9026-6e1a-4d90-ae99-92d810e998c7"),
                Title = "Board - 005",
                Description = "Board - Base - 005",
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "owner-002@email.com",
                UpdatedAt = DateTime.UtcNow,
                UpdatedBy = "owner-002@email.com"
            });

            context.SaveChanges();
        }
    }
}
