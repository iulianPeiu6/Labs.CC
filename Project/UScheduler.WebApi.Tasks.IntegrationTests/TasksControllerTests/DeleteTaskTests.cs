using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace UScheduler.WebApi.Tasks.IntegrationTests.TasksControllerTests
{
    internal class DeleteTaskTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_InexistingTaskId_When_DeleteTaskIsCalled_Then_ReturnNotFound()
        {
            var response = await testClient.DeleteAsync($"api/v1/Tasks/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Test]
        public async Task Given_ExistingTaskId_When_DeleteTaskIsCalled_Then_DeleteTask()
        {
            var response = await testClient.DeleteAsync("api/v1/Tasks/ce361462-cdb4-43c2-be2e-52003d5a4aae");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
