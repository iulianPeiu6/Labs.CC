using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UScheduler.WebApi.Users.IntegrationTests;
using UScheduler.WebApi.Workspaces.Models;

namespace UScheduler.WebApi.Workspaces.IntegrationTests.WorkspacesControllerTests
{
    public class CreateWorkspaceTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidWorkspace_When_CreateWorkspaceIsCalled_Then_ReturnCreatedWorkspaceAsync()
        {
            // Arange
            var workspace = new CreateWorkspaceModel
            {
                Owner = Guid.Parse("e27387c0-d043-4a74-861c-f9a274774974"),
                Title = "Workspace - 005",
                Description = "Workspace - 005",
                AccessType = "Private",
                ColabUsersIds = new List<string>()
                {
                    "e27387c0-d043-4a74-861c-f9a274774974"
                },
                WorkspaceType = "Basic"
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(workspace), Encoding.UTF8, "application/json");
            var response = await testClient.PostAsync("api/v1/Workspaces", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var createdWorkspace = JsonSerializer.Deserialize<WorkspaceDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            createdWorkspace.Should().NotBeNull();
            createdWorkspace?.Id.Should().NotBeEmpty();
            createdWorkspace?.Owner.Should().Be(workspace.Owner);
            createdWorkspace?.Title.Should().Be(workspace.Title);
            createdWorkspace?.Description.Should().Be(workspace.Description);
            createdWorkspace?.AccessType.Should().Be(workspace.AccessType);
            createdWorkspace?.ColabUsersIds.Should().Contain(workspace.ColabUsersIds);
            createdWorkspace?.WorkspaceType.Should().Be(workspace.WorkspaceType);
        }
    }
}
