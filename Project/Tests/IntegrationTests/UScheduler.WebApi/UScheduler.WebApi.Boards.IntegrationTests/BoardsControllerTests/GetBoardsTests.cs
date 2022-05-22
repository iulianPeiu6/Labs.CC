using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using UScheduler.WebApi.Boards.Models;

namespace UScheduler.WebApi.Boards.IntegrationTests.BoardsControllerTests
{
    public class GetBoardsTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ExistingWorkspaceId_When_GetAllBoardsIsCalled_Then_ReturnBoards()
        {
            var workspaceId = Guid.Parse("e6243037-8661-497b-af8f-4a9e965e8926");

            var response = await testClient.GetAsync($"api/v1/Boards?workspaceId={workspaceId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var boards = JsonSerializer.Deserialize<List<BoardDto>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            boards.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_InexistingWorkspaceId_When_GetAllBoardsIsCalled_Then_ReturnEmptyList()
        {
            var workspaceId = Guid.NewGuid();

            var response = await testClient.GetAsync($"api/v1/Boards?workspaceId={workspaceId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var boards = JsonSerializer.Deserialize<List<BoardDto>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            boards.Should().BeEmpty();
        }
    }
}
