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
                Owner = "owner-001@email.com",
                Title = "Workspace - 005",
                Description = "Workspace - 005",
                AccessLevel = "Private",
                Colabs = new List<string>()
                {
                    "owner-001@email.com"
                },
                WorkspaceTemplate = "Basic"
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(workspace), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/Workspaces");
            request.Headers.Add("CreatedBy", "owner-001@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);
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
            createdWorkspace?.AccessLevel.Should().Be(workspace.AccessLevel);
            createdWorkspace?.Colabs.Should().Contain(workspace.Colabs);
            createdWorkspace?.WorkspaceTemplate.Should().Be(workspace.WorkspaceTemplate);
            createdWorkspace?.CreatedBy.Should().Be("owner-001@email.com");
            createdWorkspace?.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 59));
            createdWorkspace?.UpdatedBy.Should().Be("owner-001@email.com");
            createdWorkspace?.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 59));
        }
    }
}
