using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UScheduler.WebApi.Tasks.Models.Task;

namespace UScheduler.WebApi.Tasks.IntegrationTests.TasksControllerTests
{
    internal class GetTasksTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ExistingBoardId_When_GetAllTasksIsCalled_Then_ReturnTasks()
        {
            var boardId = Guid.Parse("43ae9026-6e1a-4d90-ae99-92d810e998c7");

            var response = await testClient.GetAsync($"api/v1/Tasks?boardId={boardId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var tasks = JsonSerializer.Deserialize<List<TaskDto>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            tasks.Should().NotBeEmpty();
        }

        [Test]
        public async Task Given_InexistingBoardId_When_GetAllTasksIsCalled_Then_ReturnEmptyList()
        {
            var boardId = Guid.NewGuid();

            var response = await testClient.GetAsync($"api/v1/Tasks?boardId={boardId}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var tasks = JsonSerializer.Deserialize<List<TaskDto>>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            tasks.Should().BeEmpty();
        }
    }
}
