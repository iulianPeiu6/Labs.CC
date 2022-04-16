using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using UScheduler.WebApi.Users.Models;

namespace UScheduler.WebApi.Users.IntegrationTests.UsersControllerTests
{
    public class GetUserByIdTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidUserId_When_GetUserByIdIsCalled_Then_ReturnUserAsync()
        {
            // Arange
            var userId = Guid.Parse("4b04a881-ce69-4787-b25a-76460a639f3f");

            // Act
            var response = await testClient.GetAsync($"api/v1/Users/{userId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<DisplayUserModel>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            user.Should().NotBeNull();
            user?.Id.Should().NotBeEmpty();
            user?.UserName.Should().NotBeNullOrEmpty();
            user?.Email.Should().NotBeNullOrEmpty();
            user?.RegistrationDate.Should().NotBeSameDateAs(DateTime.MinValue);
        }

        [Test]
        public async Task Given_InvalidUserId_When_GetUserByIdIsCalled_Then_ReturnNotFoundAsync()
        {
            // Arange
            var userId = inexistentUserId;

            // Act
            var response = await testClient.GetAsync($"api/v1/Users/{userId}");

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
