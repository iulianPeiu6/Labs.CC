using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Threading.Tasks;
using UScheduler.WebApi.Users.IntegrationTests;

namespace UScheduler.WebApi.Workspaces.IntegrationTests.WorkspacesControllerTests
{
    public class DeleteWorkspaceTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidWorkspaceId_When_DeleteWorkspaceIsCalled_Then_ReturnOkAsync()
        {
            // Arange
            var id = Guid.Parse("0abf0eb1-072c-44b0-a775-7141d9d852c8");

            // Act
            var response = await testClient.DeleteAsync($"api/v1/Workspaces/{id}");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Given_InvalidWorkspaceId_When_DeleteWorkspaceIsCalled_Then_ReturnNotFoundAsync()
        {
            // Arange
            var id = inexistentWorkspceId;

            // Act
            var response = await testClient.DeleteAsync($"api/v1/Workspaces/{id}");
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
