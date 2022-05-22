using FluentAssertions;
using NUnit.Framework;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using UScheduler.WebApi.Boards.Models;

namespace UScheduler.WebApi.Boards.IntegrationTests.BoardsControllerTests
{
    public class GetBoardTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ExistingBoardId_When_GetBoardIsCalled_Then_ReturnBoard()
        {
            var response = await testClient.GetAsync($"api/v1/Boards/{existentBoardId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var board = JsonSerializer.Deserialize<BoardDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            board.Should().NotBeNull();
            board.Title.Should().Be("Board - 001");
        }

        [Test]
        public async Task Given_InexistingBoardId_When_GetBoardIsCalled_Then_ReturnNotFound()
        {
            var response = await testClient.GetAsync($"api/v1/Boards/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
