using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;

namespace UScheduler.WebApi.Users.IntegrationTests.UsersControllerTests
{
    public class DeleteUserTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidUserId_When_DeleteUserIsCalled_Then_ReturnNotFoundAsync()
        {
            // Arange
            var userId = Guid.Parse("5ac72a84-e32c-4a75-a696-af1f54bdd35c");

            // Act
            var response = await testClient.DeleteAsync($"api/v1/Users/{userId}");

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Given_InvalidUserId_When_DeleteUserIsCalled_Then_ReturnNotFoundAsync()
        {
            // Arange
            var userId = inexistentUserId;

            // Act
            var response = await testClient.DeleteAsync($"api/v1/Users/{userId}");

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
