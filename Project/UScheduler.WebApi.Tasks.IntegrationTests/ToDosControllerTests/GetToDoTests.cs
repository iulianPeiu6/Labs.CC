using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UScheduler.WebApi.Tasks.Models.ToDo;

namespace UScheduler.WebApi.Tasks.IntegrationTests.ToDosControllerTests
{
    internal class GetToDoTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ExistingTaskId_When_GetTaskIsCalled_Then_ReturnTask()
        {
            var response = await testClient.GetAsync("api/v1/ToDos/58caaac9-84f4-4bd4-9c91-9c22bfb47a1a");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var todo = JsonSerializer.Deserialize<ToDoDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            todo.Should().NotBeNull();
            todo.Description.Should().Be("ToDo description - 001");
            todo.CreatedBy = "owner-001@email.com";
            todo.UpdatedBy = "owner-001@email.com";
            todo.CreatedAt = currentDateTime;
            todo.UpdatedAt = currentDateTime;
        }

        [Test]
        public async Task Given_InexistingToDo_When_GetToDoIsCalled_Then_ReturnNotFound()
        {
            var response = await testClient.GetAsync($"api/v1/ToDos/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
