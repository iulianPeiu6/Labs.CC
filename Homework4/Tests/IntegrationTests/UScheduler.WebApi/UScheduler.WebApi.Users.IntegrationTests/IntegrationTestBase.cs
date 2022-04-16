using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Net.Http;
using UScheduler.WebApi.Users.Data;
using UScheduler.WebApi.Users.Data.Entities;

namespace UScheduler.WebApi.Users.IntegrationTests
{
    public class IntegrationTestBase
    {
        private static IntegrationTestBase instance = new IntegrationTestBase();
        protected readonly HttpClient testClient;
        protected readonly Guid inexistentUserId = Guid.Parse("79cf1055-efa8-4dab-80ef-c9a59d4adcb1");

        protected IntegrationTestBase()
        {
            if (instance is null)
            {
                var appFactory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContextOptions<UsersContext>));
                        services.AddDbContext<UsersContext>(options =>
                        {
                            options.UseInMemoryDatabase($"UsersTestDb");
                        });
                    });
                });

                var scopeFactory = appFactory.Server.Services.GetService<IServiceScopeFactory>();
                using (var scope = scopeFactory?.CreateScope())
                {
                    var context = scope?.ServiceProvider?.GetRequiredService<UsersContext>();
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

        private static void SeedRecordsInDatabase(UsersContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.Database.EnsureCreated();

            context.Add(new User
            {
                Id = Guid.Parse("4b04a881-ce69-4787-b25a-76460a639f3f"),
                UserName = "username-001",
                Email = "email.001@email.com",
                RegistrationDate = DateTime.Now,
                HashedPassword = "AABBCCDD981A=",
                AccountSettings = new AccountSettings()
                {
                    EmailForNotification = "email.001@email.com",
                    SendNotificationOnEmail = false
                }
            });

            context.Add(new User
            {
                Id = Guid.Parse("5ac72a84-e32c-4a75-a696-af1f54bdd35c"),
                UserName = "username-002",
                Email = "email.002@email.com",
                RegistrationDate = DateTime.Now,
                HashedPassword = "AABBCCDD981B=",
                AccountSettings = new AccountSettings()
                {
                    EmailForNotification = "email.002@email.com",
                    SendNotificationOnEmail = false
                }
            });

            context.Add(new User
            {
                Id = Guid.Parse("8dae4f26-7404-4dc8-9d20-03aab065f7cd"),
                UserName = "username-003",
                Email = "email.003@email.com",
                RegistrationDate = DateTime.Now,
                HashedPassword = "AABBCCDD981B=",
                AccountSettings = new AccountSettings()
                {
                    EmailForNotification = "email.003@email.com",
                    SendNotificationOnEmail = false
                }
            });

            context.Add(new User
            {
                Id = Guid.Parse("2b0f8ed4-0e9d-489f-b003-e34fa24a8f8a"),
                UserName = "username-004",
                Email = "email.004@email.com",
                RegistrationDate = DateTime.Now,
                HashedPassword = "AABBCCDD981B=",
                AccountSettings = new AccountSettings()
                {
                    EmailForNotification = "email.004@email.com",
                    SendNotificationOnEmail = false
                }
            });

            context.SaveChanges();
        }
    }
}
