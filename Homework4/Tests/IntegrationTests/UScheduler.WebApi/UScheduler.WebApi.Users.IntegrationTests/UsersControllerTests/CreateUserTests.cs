using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UScheduler.WebApi.Users.Models;

namespace UScheduler.WebApi.Users.IntegrationTests.UsersControllerTests
{
    public class CreateUserTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidUser_When_CreateUserIsCalled_Then_ReturnCreatedUserAsync()
        {
            // Arange
            var user = new CreateUserModel
            {
                UserName = "username-005",
                Email = "email.005@email.com",
                HashedPassword = "ABC1ABC9="
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await testClient.PostAsync("api/v1/Users", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var createdUser = JsonSerializer.Deserialize<DisplayUserModel>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            createdUser.Should().NotBeNull();
            createdUser?.Id.Should().NotBeEmpty();
            createdUser?.RegistrationDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromDays(1));
            createdUser?.UserName.Should().Be(user.UserName);
            createdUser?.Email.Should().Be(user.Email);
            createdUser?.AccountSettings.Should().NotBeNull();
            createdUser?.AccountSettings?.EmailForNotification.Should().Be(user.Email);
            createdUser?.AccountSettings?.SendNotificationOnEmail.Should().Be(false);
        }

        [Test]
        public async Task Given_UserWithInvalidEmail_When_CreateUserIsCalled_Then_Return400BadRequestUserAsync()
        {
            // Arange
            var user = new CreateUserModel
            {
                UserName = "username-005",
                Email = "invalid_email",
                HashedPassword = "ABC1ABC9="
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await testClient.PostAsync("api/v1/Users", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_UserWithAlreadyUsedEmail_When_CreateUserIsCalled_Then_Return409ConflictAsync()
        {
            // Arange
            var user = new CreateUserModel
            {
                UserName = "username-005",
                Email = "email.003@email.com",
                HashedPassword = "ABC1ABC9="
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await testClient.PostAsync("api/v1/Users", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_UserWithAlreadyUsedUsername_When_CreateUserIsCalled_Then_Return409ConflictAsync()
        {
            // Arange
            var user = new CreateUserModel
            {
                UserName = "username-003",
                Email = "email.005@email.com",
                HashedPassword = "ABC1ABC9="
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await testClient.PostAsync("api/v1/Users", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_UserWithEmptyUsername_When_CreateUserIsCalled_Then_Return400BadRequestUserAsync()
        {
            // Arange
            var user = new CreateUserModel
            {
                UserName = "",
                Email = "email.006@email.com",
                HashedPassword = "ABC1ABC9="
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await testClient.PostAsync("api/v1/Users", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseContent.Should().NotBeNullOrEmpty();
        }

        [Test]
        public async Task Given_UserWithEmptyhashedPassword_When_CreateUserIsCalled_Then_Return400BadRequestUserAsync()
        {
            // Arange
            var user = new CreateUserModel
            {
                UserName = "username",
                Email = "email.005@email.com",
                HashedPassword = ""
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");
            var response = await testClient.PostAsync("api/v1/Users", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseContent.Should().NotBeNullOrEmpty();
        }
    }
}
