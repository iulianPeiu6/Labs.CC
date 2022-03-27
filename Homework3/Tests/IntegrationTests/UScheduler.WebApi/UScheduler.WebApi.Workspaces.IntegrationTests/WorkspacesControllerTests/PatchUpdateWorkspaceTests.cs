using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UScheduler.WebApi.Users.IntegrationTests;
using UScheduler.WebApi.Workspaces.Data.Entities;
using UScheduler.WebApi.Workspaces.Models;

namespace UScheduler.WebApi.Workspaces.IntegrationTests.WorkspacesControllerTests
{
    public class PatchUpdateWorkspaceTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_InexistentWorkspaceId_When_PartialUpdatedWorkspaceIsCalled_Then_ReturnNotFoundAsync()
        {
            // Arange
            var id = inexistentWorkspceId;
            var patchDoc = new JsonPatchDocument<Workspace>();

            // Act
            var requestContent = new StringContent(JsonConvert.SerializeObject(patchDoc), Encoding.UTF8, "application/json-patch+json");
            var response = await testClient.PatchAsync($"api/v1/Workspaces/{id}", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Given_ValidWorkspaceIdWithValidUpdateRequest_When_PartialUpdatedWorkspaceIsCalled_Then_ReturnOkWithUpdatedWorkspaceAsync()
        {
            // Arange
            var id = Guid.Parse("367c9423-0d7a-49d5-8376-5619804271bf");
            var patchDoc = new JsonPatchDocument<Workspace>();
            patchDoc.Replace(w => w.Title, "Workspace - 002 - Patch - Updated");
            patchDoc.Replace(w => w.Description, "Workspace - 002 - Patch - Updated");
            patchDoc.Replace(w => w.Owner, Guid.Parse("508bb148-f2eb-4a61-8a1b-f16a38eda698"));
            patchDoc.Replace(w => w.AccessType, "Private");
            patchDoc.Add(w => w.ColabUsersIds, "efc9ad1f-1c51-4406-ab96-e50556849054");

            // Act
            var requestContent = new StringContent(JsonConvert.SerializeObject(patchDoc), Encoding.UTF8, "application/json-patch+json");
            var response = await testClient.PatchAsync($"api/v1/Workspaces/{id}", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedWorkspace = System.Text.Json.JsonSerializer.Deserialize<WorkspaceDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedWorkspace.Should().NotBeNull();
            updatedWorkspace?.Id.Should().Be(Guid.Parse("367c9423-0d7a-49d5-8376-5619804271bf"));
            updatedWorkspace?.Owner.Should().Be(Guid.Parse("508bb148-f2eb-4a61-8a1b-f16a38eda698"));
            updatedWorkspace?.Title.Should().Be("Workspace - 002 - Patch - Updated");
            updatedWorkspace?.Description.Should().Be("Workspace - 002 - Patch - Updated");
            updatedWorkspace?.AccessType.Should().Be("Private");
            updatedWorkspace?.ColabUsersIds.Should().Contain("efc9ad1f-1c51-4406-ab96-e50556849054");
        }
    }
}
