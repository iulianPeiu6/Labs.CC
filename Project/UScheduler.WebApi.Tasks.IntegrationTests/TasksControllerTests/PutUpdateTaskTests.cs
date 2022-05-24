using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UScheduler.WebApi.Tasks.Models.Task;

namespace UScheduler.WebApi.Tasks.IntegrationTests.TasksControllerTests
{
    internal class PutUpdateTaskTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidBoard_When_PutUpdateBoardIsCalled_Then_ReturnOkWithUpdatedBoard()
        {
            var taskId = Guid.Parse("7c139c88-48d6-4233-8584-4db3389cf3e1");
            var newDueDate = DateTime.UtcNow.AddDays(7);
            var task = new UpdateTaskModel
            {
                Title = "Task title - 001 - Updated PUT",
                Description = "Task description - 001 - Updated PUT",
                DueDateTime = newDueDate
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/v1/Tasks/{taskId}");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedTask = JsonSerializer.Deserialize<TaskDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            updatedTask.Should().NotBeNull();
            updatedTask.Title.Should().Be("Task title - 001 - Updated PUT");
            updatedTask.Description.Should().Be("Task description - 001 - Updated PUT");
            updatedTask.UpdatedBy.Should().Be("integration_test@email.com");
            updatedTask.DueDateTime = newDueDate;
        }

        [Test]
        public async Task Given_InvalidTask_When_PutUpdateTaskIsCalled_Then_ReturnNotFound()
        {
            var task = new UpdateTaskModel();

            var requestContent = new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/v1/Tasks/{Guid.NewGuid()}");
            request.Content = requestContent;

            var response = await testClient.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
