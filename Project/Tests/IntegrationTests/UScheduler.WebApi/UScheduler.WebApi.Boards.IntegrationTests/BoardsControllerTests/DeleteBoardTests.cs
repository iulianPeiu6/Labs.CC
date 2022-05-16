using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace UScheduler.WebApi.Boards.IntegrationTests.BoardsControllerTests
{
    public class DeleteBoardTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_InexistingBoardId_When_DeleteBoardIsCalled_Then_ReturnNotFound()
        {
            var boardId = Guid.NewGuid();
            var response = await testClient.DeleteAsync($"api/v1/Boards/{boardId}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Given_ExistingBoardId_When_DeleteBoardIsCalled_Then_DeleteBoard()
        {
            var response = await testClient.DeleteAsync($"api/v1/Boards/{existentBoardId}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
