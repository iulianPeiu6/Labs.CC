using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UScheduler.WebApi.Tasks.Models.ToDo;

namespace UScheduler.WebApi.Tasks.IntegrationTests.ToDosControllerTests
{
    internal class GetToDosTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ExistingTaskId_When_GetToDosIsCalled_Then_ReturnToDos()
        {
            var response = await testClient.GetAsync("api/v1/ToDos?taskId=5939ca2e-e9de-4d13-ab6a-81461422cd1b");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var toDos = JsonSerializer.Deserialize<IEnumerable<ToDoDto>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            toDos.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_InexistingToDo_When_GetToDoIsCalled_Then_ReturnNotFound()
        {
            var response = await testClient.GetAsync($"api/v1/ToDos?taskId={Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var toDos = JsonSerializer.Deserialize<IEnumerable<ToDoDto>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            toDos.Should().BeEmpty();
        }
    }
}
