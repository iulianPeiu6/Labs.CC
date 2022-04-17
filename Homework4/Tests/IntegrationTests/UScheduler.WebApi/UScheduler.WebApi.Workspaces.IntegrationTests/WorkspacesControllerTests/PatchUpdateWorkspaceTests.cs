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
            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/v1/Workspaces/{id}");
            request.Headers.Add("UpdatedBy", "owner-new-003@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);
            await response.Content.ReadAsStringAsync();

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
            patchDoc.Replace(w => w.Owner, "owner-new-003@email.com");
            patchDoc.Replace(w => w.AccessType, "Private");
            patchDoc.Add(w => w.Colabs, "owner-new-004@email.com");

            // Act
            var requestContent = new StringContent(JsonConvert.SerializeObject(patchDoc), Encoding.UTF8, "application/json-patch+json");
            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/v1/Workspaces/{id}");
            request.Headers.Add("UpdatedBy", "owner-new-003@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedWorkspace = System.Text.Json.JsonSerializer.Deserialize<WorkspaceDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedWorkspace.Should().NotBeNull();
            updatedWorkspace?.Id.Should().Be(Guid.Parse("367c9423-0d7a-49d5-8376-5619804271bf"));
            updatedWorkspace?.Owner.Should().Be("owner-new-003@email.com");
            updatedWorkspace?.Title.Should().Be("Workspace - 002 - Patch - Updated");
            updatedWorkspace?.Description.Should().Be("Workspace - 002 - Patch - Updated");
            updatedWorkspace?.AccessType.Should().Be("Private");
            updatedWorkspace?.Colabs.Should().Contain("owner-new-004@email.com");
            updatedWorkspace?.UpdatedBy.Should().Contain("owner-new-003@email.com");
            updatedWorkspace?.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, new TimeSpan(0, 0, 59));
        }

        [Test]
        public async Task Given_ValidWorkspaceIdWithUpdatedByHeaderMissingt_When_PartialUpdatedWorkspaceIsCalled_Then_Return500InternalServerErrorAsync()
        {
            // Arange
            var id = Guid.Parse("367c9423-0d7a-49d5-8376-5619804271bf");
            var patchDoc = new JsonPatchDocument<Workspace>();
            patchDoc.Replace(w => w.Title, "Workspace - 002 - Patch - Updated");
            patchDoc.Replace(w => w.Description, "Workspace - 002 - Patch - Updated");
            patchDoc.Replace(w => w.Owner, "owner-new-003@email.com");
            patchDoc.Replace(w => w.AccessType, "Private");
            patchDoc.Add(w => w.Colabs, "owner-new-004@email.com");

            // Act
            var requestContent = new StringContent(JsonConvert.SerializeObject(patchDoc), Encoding.UTF8, "application/json-patch+json");
            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/v1/Workspaces/{id}");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);
            await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
    }
}
