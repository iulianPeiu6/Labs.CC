using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UScheduler.WebApi.Users.IntegrationTests;
using UScheduler.WebApi.Workspaces.Models;

namespace UScheduler.WebApi.Workspaces.IntegrationTests.WorkspacesControllerTests
{
    public class PutUpdateWorkspaceTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidWorkspace_When_CreateWorkspaceIsCalled_Then_ReturnCreatedWorkspaceAsync()
        {
            // Arange
            var id = inexistentWorkspceId;
            var workspace = new UpdateWorkspaceModel
            {
                Owner = Guid.Parse("c89a5809-9e53-4950-b2ac-9582f6813160"),
                Title = "Workspace - 002 - Updated",
                Description = "Workspace - Update - 002 - Updated",
                AccessType = "Public",
                WorkspaceType = "Education"
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(workspace), Encoding.UTF8, "application/json");
            var response = await testClient.PutAsync($"api/v1/Workspaces/{id}", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Given_ValidWorkspaceIdWithValidUpdateRequest_When_FullUpdatedWorkspaceIsCalled_Then_ReturnOkWithUpdatedWorkspaceAsync()
        {
            // Arange
            var id = Guid.Parse("367c9423-0d7a-49d5-8376-5619804271bf");
            var workspace = new UpdateWorkspaceModel
            {
                Owner = Guid.Parse("c89a5809-9e53-4950-b2ac-9582f6813160"),
                Title = "Workspace - 002 - Updated",
                Description = "Workspace - Update - 002 - Updated",
                AccessType = "Public",
                WorkspaceType = "Education"
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(workspace), Encoding.UTF8, "application/json");
            var response = await testClient.PutAsync($"api/v1/Workspaces/{id}", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedWorkspace = JsonSerializer.Deserialize<WorkspaceDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedWorkspace.Should().NotBeNull();
            updatedWorkspace?.Id.Should().NotBeEmpty();
            updatedWorkspace?.Owner.Should().Be(workspace.Owner);
            updatedWorkspace?.Title.Should().Be(workspace.Title);
            updatedWorkspace?.Description.Should().Be(workspace.Description);
            updatedWorkspace?.AccessType.Should().Be(workspace.AccessType);
            updatedWorkspace?.WorkspaceType.Should().Be(workspace.WorkspaceType);
        }
    }
}
