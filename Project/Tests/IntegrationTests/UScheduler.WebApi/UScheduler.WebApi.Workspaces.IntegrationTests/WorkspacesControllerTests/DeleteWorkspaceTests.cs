using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
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
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/v1/Workspaces/{id}");
            request.Headers.Add("RequestedBy", "owner-001@email.com");
            var response = await testClient.SendAsync(request);
            await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Test]
        public async Task Given_InvalidWorkspaceId_When_DeleteWorkspaceIsCalled_Then_ReturnNotFoundAsync()
        {
            // Arange
            var id = inexistentWorkspceId;

            // Act
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/v1/Workspaces/{id}");
            request.Headers.Add("RequestedBy", "owner-001@email.com");
            var response = await testClient.SendAsync(request);
            await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Given_ValidWorkspaceId_When_DeleteWorkspaceIsCalledWithRequestedByHeaderMissing_Then_ReturnInternalErrorAsync()
        {
            // Arange
            var id = Guid.Parse("0abf0eb1-072c-44b0-a775-7141d9d852c8");

            // Act
            var request = new HttpRequestMessage(HttpMethod.Delete, $"api/v1/Workspaces/{id}");
            var response = await testClient.SendAsync(request);
            await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
