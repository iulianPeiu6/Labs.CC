using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UScheduler.WebApi.Users.Data.Entities;
using UScheduler.WebApi.Users.Models;

namespace UScheduler.WebApi.Users.IntegrationTests.UsersControllerTests
{
    public class PutUpdateUserTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidUserIdWithValidUpdate_When_PutUpdateUserIsCalled_Then_ReturnOkWithTheUpdatedUserAsync()
        {
            // Arange
            var userId = Guid.Parse("2b0f8ed4-0e9d-489f-b003-e34fa24a8f8a");
            var user = new UpdateUserModel
            {
                UserName = "username-004-updated",
                Email = "email.004.updated@email.com",
                HashedPassword = "ABB67BD45=",
                AccountSettings = new AccountSettings
                {
                    EmailForNotification = "email.004.updated@email.com",
                    SendNotificationOnEmail = true
                }
            };
            var requestBody = JsonSerializer.Serialize(user);

            // Act
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await testClient.PutAsync($"api/v1/Users/{userId}", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedUser = JsonSerializer.Deserialize<DisplayUserModel>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedUser.Should().NotBeNull();
            updatedUser?.Id.Should().Be(userId);
            updatedUser?.UserName.Should().Be(user.UserName);
            updatedUser?.Email.Should().Be(user.Email);
            updatedUser?.AccountSettings.Should().NotBeNull();
            updatedUser?.AccountSettings?.EmailForNotification.Should().Be(user.Email);
            updatedUser?.AccountSettings?.SendNotificationOnEmail.Should().Be(user.AccountSettings.SendNotificationOnEmail);
        }

        [Test]
        public async Task Given_InexistentUserId_When_PutUpdateUserIsCalled_Then_Return404NotFoundAsync()
        {
            // Arange
            var userId = inexistentUserId;
            var user = new UpdateUserModel
            {
                UserName = "username-004-updated",
                Email = "email.004.updated@email.com",
                HashedPassword = "ABB67BD45=",
                AccountSettings = new AccountSettings
                {
                    EmailForNotification = "email.004.updated@email.com",
                    SendNotificationOnEmail = true
                }
            };
            var requestBody = JsonSerializer.Serialize(user);

            // Act
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await testClient.PutAsync($"api/v1/Users/{userId}", requestContent);

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Given_ValidUserIdWithInvalidUpdateTakenUsername_When_PutUpdateUserIsCalled_Then_Return409ConflictAsync()
        {
            // Arange
            var userId = Guid.Parse("2b0f8ed4-0e9d-489f-b003-e34fa24a8f8a");
            var user = new UpdateUserModel
            {
                UserName = "username-003",
                Email = "email.004.updated@email.com",
                HashedPassword = "ABB67BD45=",
                AccountSettings = new AccountSettings
                {
                    EmailForNotification = "email.004.updated@email.com",
                    SendNotificationOnEmail = true
                }
            };
            var requestBody = JsonSerializer.Serialize(user);

            // Act
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await testClient.PutAsync($"api/v1/Users/{userId}", requestContent);

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Test]
        public async Task Given_ValidUserIdWithInvalidUpdateTakenEmail_When_PutUpdateUserIsCalled_Then_Return409ConflictAsync()
        {
            // Arange
            var userId = Guid.Parse("2b0f8ed4-0e9d-489f-b003-e34fa24a8f8a");
            var user = new UpdateUserModel
            {
                UserName = "username-004-updated",
                Email = "email.003@email.com",
                HashedPassword = "ABB67BD45=",
                AccountSettings = new AccountSettings
                {
                    EmailForNotification = "email.004.updated@email.com",
                    SendNotificationOnEmail = true
                }
            };
            var requestBody = JsonSerializer.Serialize(user);

            // Act
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await testClient.PutAsync($"api/v1/Users/{userId}", requestContent);

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Test]
        public async Task Given_ValidUserIdWithInvalidUpdateEmptyEmail_When_PutUpdateUserIsCalled_Then_Return400BadRequestAsync()
        {
            // Arange
            var userId = Guid.Parse("2b0f8ed4-0e9d-489f-b003-e34fa24a8f8a");
            var user = new UpdateUserModel
            {
                UserName = "",
                Email = "email.004.updated@email.com",
                HashedPassword = "ABB67BD45=",
                AccountSettings = new AccountSettings
                {
                    EmailForNotification = "email.004.updated@email.com",
                    SendNotificationOnEmail = true
                }
            };
            var requestBody = JsonSerializer.Serialize(user);

            // Act
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await testClient.PutAsync($"api/v1/Users/{userId}", requestContent);

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task Given_ValidUserIdWithInvalidUpdateInvalidEmail_When_PutUpdateUserIsCalled_Then_Return400BadRequestAsync()
        {
            // Arange
            var userId = Guid.Parse("2b0f8ed4-0e9d-489f-b003-e34fa24a8f8a");
            var user = new UpdateUserModel
            {
                UserName = "username-004-updated",
                Email = "invalid_email",
                HashedPassword = "ABB67BD45=",
                AccountSettings = new AccountSettings
                {
                    EmailForNotification = "email.004.updated@email.com",
                    SendNotificationOnEmail = true
                }
            };
            var requestBody = JsonSerializer.Serialize(user);

            // Act
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json");
            var response = await testClient.PutAsync($"api/v1/Users/{userId}", requestContent);

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
