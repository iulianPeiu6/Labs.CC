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
using UScheduler.WebApi.Users.Models;

namespace UScheduler.WebApi.Users.IntegrationTests.UsersControllerTests
{
    public class PatchUpdateUserTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_InvalidUser_When_PatchUpdateUserIsCalled_Then_Return404NotFoundAsync()
        {
            // Arange
            var userId = inexistentUserId;
            var patchDoc = new JsonPatchDocument<UpdateUserModel>();
            patchDoc.Replace(u => u.UserName, "username-new");
            var requestBody = JsonConvert.SerializeObject(patchDoc);

            // Act
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json-patch+json");
            var response = await testClient.PatchAsync($"api/v1/Users/{userId}", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Given_ValidUserWithConflictUsernameUpdate_When_PatchUpdateUserIsCalled_Then_ReturnConflictAsync()
        {
            // Arange
            var userId = Guid.Parse("4b04a881-ce69-4787-b25a-76460a639f3f");
            var patchDoc = new JsonPatchDocument<UpdateUserModel>();
            patchDoc.Replace(u => u.UserName, "username-003");
            var requestBody = JsonConvert.SerializeObject(patchDoc);

            // Act
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json-patch+json");
            var response = await testClient.PatchAsync($"api/v1/Users/{userId}", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Test]
        public async Task Given_ValidUserWithConflictEmailUpdate_When_PatchUpdateUserIsCalled_Then_ReturnConflictAsync()
        {
            // Arange
            var userId = Guid.Parse("4b04a881-ce69-4787-b25a-76460a639f3f");
            var patchDoc = new JsonPatchDocument<UpdateUserModel>();
            patchDoc.Replace(u => u.Email, "email.003@email.com");
            var requestBody = JsonConvert.SerializeObject(patchDoc);

            // Act
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json-patch+json");
            var response = await testClient.PatchAsync($"api/v1/Users/{userId}", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Test]
        public async Task Given_ValidUserWithValidUpdateRequest_When_PatchUpdateUserIsCalled_Then_Return200OkAsync()
        {
            // Arange
            var userId = Guid.Parse("4b04a881-ce69-4787-b25a-76460a639f3f");
            var patchDoc = new JsonPatchDocument<UpdateUserModel>();
            patchDoc.Replace(u => u.UserName, "username-new");
            patchDoc.Replace(u => u.Email, "email.new@email.com");
            var requestBody = JsonConvert.SerializeObject(patchDoc);

            // Act
            var requestContent = new StringContent(requestBody, Encoding.UTF8, "application/json-patch+json");
            var response = await testClient.PatchAsync($"api/v1/Users/{userId}", requestContent);
            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedUser = System.Text.Json.JsonSerializer.Deserialize<DisplayUserModel>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Asert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            updatedUser?.UserName.Should().Be("username-new");
            updatedUser?.Email.Should().Be("email.new@email.com");
        }
    }
}
