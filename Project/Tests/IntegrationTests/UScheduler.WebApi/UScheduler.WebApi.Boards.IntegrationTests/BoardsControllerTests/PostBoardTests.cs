using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UScheduler.WebApi.Boards.Models;

namespace UScheduler.WebApi.Boards.IntegrationTests.BoardsControllerTests
{
    internal class PostBoardTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_InexistentBoard_WhenCreateTask_ShouldReturnBadRequest()
        {
            var task = new CreateBoardModel()
            {
                Description = "Task created via POST",
                Title = "Task created via POST",
                BoardTemplate = "Default",
                WorkspaceId = Guid.NewGuid()
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/Boards");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
