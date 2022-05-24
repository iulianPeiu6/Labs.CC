using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using NUnit.Framework;
using UScheduler.WebApi.Boards.Data.Entities;
using UScheduler.WebApi.Boards.Models;

namespace UScheduler.WebApi.Boards.IntegrationTests.BoardsControllerTests
{
    internal class PatchUpdateBoardTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidBoard_When_PatchUpdateBoardIsCalled_Then_ReturnOkWithBoard()
        {
            var todo = new JsonPatchDocument<Board>();

            todo.Replace(t => t.Description, "Board description patch updated");

            var requestContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json-patch+json");
            var request = new HttpRequestMessage(HttpMethod.Patch, "api/v1/Boards/63c56e16-6a65-411b-9899-647c40b7bf0e");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);

            //response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedTask = System.Text.Json.JsonSerializer.Deserialize<BoardDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            updatedTask.Should().NotBeNull();
            updatedTask.Description.Should().Be("Board description patch updated");
            updatedTask.UpdatedBy.Should().Be("integration_test@email.com");
        }

        [Test]
        public async Task Given_InvalidBoard_When_PutUpdateBoardIsCalled_Then_ReturnNotFound()
        {
            var todo = new JsonPatchDocument<Board>();

            todo.Replace(t => t.Description, "Task description - 003 patch updated");

            var requestContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json-patch+json");
            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/v1/Boards/{Guid.NewGuid()}");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;

            var response = await testClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
