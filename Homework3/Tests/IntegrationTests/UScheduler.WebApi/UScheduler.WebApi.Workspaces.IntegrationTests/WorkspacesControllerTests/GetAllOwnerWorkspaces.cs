using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using UScheduler.WebApi.Users.IntegrationTests;
using UScheduler.WebApi.Workspaces.Models;

namespace UScheduler.WebApi.Workspaces.IntegrationTests.WorkspacesControllerTests
{
    public class GetAllOwnerWorkspaces : IntegrationTestBase
    {
        [Test]
        public async Task Given_OwnerId_When_GetAllOwnerWorkspacesIsCalled_Then_ReturnOwnerWorkspacesAsync()
        {
            // Arange
            var ownerId = Guid.Parse("e27387c0-d043-4a74-861c-f9a274774974");

            // Act
            var response = await testClient.GetAsync($"api/v1/Workspaces/GroupedByOwners/{ownerId}");
            var responseContent = await response.Content.ReadAsStringAsync();
            var ownerWorkspaces = JsonSerializer.Deserialize<List<WorkspaceDto>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            ownerWorkspaces.Should().NotBeNull();
            foreach (var workspace in ownerWorkspaces)
            {
                workspace?.Id.Should().NotBeEmpty();
                workspace?.Owner.Should().NotBeEmpty();
                workspace?.Title.Should().NotBeNullOrEmpty();
                workspace?.Description.Should().NotBeNullOrEmpty();
                workspace?.AccessType.Should().NotBeNullOrEmpty();
                workspace?.ColabUsersIds.Should().NotBeNullOrEmpty();
                workspace?.WorkspaceType.Should().NotBeNullOrEmpty();
            }
        }
    }
}
