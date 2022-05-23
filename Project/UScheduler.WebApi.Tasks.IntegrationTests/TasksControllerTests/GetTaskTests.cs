using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UScheduler.WebApi.Tasks.Models.Task;

namespace UScheduler.WebApi.Tasks.IntegrationTests.TasksControllerTests
{
    internal class GetTaskTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ExistingTaskId_When_GetTaskIsCalled_Then_ReturnTask()
        {
            var response = await testClient.GetAsync($"api/v1/Tasks/{existentTaskId}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var task = JsonSerializer.Deserialize<TaskDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            task.Should().NotBeNull();
            task.Title.Should().Be("Task title - 001");
            task.Description.Should().Be("Task description - 001");
            task.DueDateTime = dueDateTime;
            task.BoardId = Guid.Parse("43ae9026-6e1a-4d90-ae99-92d810e998c7");
            task.CreatedBy = "owner-001@email.com";
            task.UpdatedBy = "owner-001@email.com";
            task.CreatedAt = currentDateTime;
            task.UpdatedAt = currentDateTime;
        }

        [Test]
        public async Task Given_InexistingTaskId_When_GetTaskIsCalled_Then_ReturnNotFound()
        {
            var response = await testClient.GetAsync($"api/v1/Tasks/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
