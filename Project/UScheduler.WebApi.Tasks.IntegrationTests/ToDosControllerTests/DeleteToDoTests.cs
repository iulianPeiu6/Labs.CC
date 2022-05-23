using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace UScheduler.WebApi.Tasks.IntegrationTests.ToDosControllerTests
{
    public class DeleteToDoTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_InexistingToDoId_When_DeleteToDoIsCalled_Then_ReturnNotFound()
        {
            var response = await testClient.DeleteAsync($"api/v1/ToDos/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Given_ExistingToDo_When_DeleteToDoIsCalled_Then_DeleteToDo()
        {
            var response = await testClient.DeleteAsync("api/v1/ToDos/c24563c5-7b21-4f04-b807-fb368673dc33");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
