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
    public class PutUpdateBoardTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidBoard_When_PutUpdateBoardIsCalled_Then_ReturnOkWithUpdatedBoard()
        {
            var boardId = Guid.Parse("5c675d5d-9c94-48cd-8c35-bfaaae334691");
            var board = new UpdateBoardModel()
            {
                Title = "Workspace - 001 - Updated",
                Description = "Workspace - Update - 001 - Updated"
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(board), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/v1/Boards/{boardId}");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedBoard = JsonSerializer.Deserialize<BoardDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            updatedBoard.Should().NotBeNull();
            updatedBoard.Title.Should().Be("Workspace - 001 - Updated");
            updatedBoard.Description.Should().Be("Workspace - Update - 001 - Updated");
            updatedBoard.UpdatedBy.Should().Be("integration_test@email.com");
        }
    }
}
