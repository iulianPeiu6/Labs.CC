using FluentAssertions;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using UScheduler.WebApi.Users.Models;

namespace UScheduler.WebApi.Users.IntegrationTests.UsersControllerTests
{
    public class GetAllUserTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_GetRequest_When_GetAllIsCalled_Then_ReturnAllUsersAsync()
        {
            // Arange

            // Act
            var response = await testClient.GetAsync("api/v1/Users");
            var responseContent = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<DisplayUserModel>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            users.Should().NotBeEmpty();
        }
    }
}
