using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UScheduler.WebApi.Tasks.Models.Task;
using UScheduler.WebApi.Tasks.Models.ToDo;

namespace UScheduler.WebApi.Tasks.IntegrationTests.TasksControllerTests
{
    internal class PostTaskTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_InexistentBoard_WhenCreateTask_ShouldReturnBadRequest()
        {
            var task = new CreateTaskModel
            {
                Description = "Task created via POST",
                BoardId = Guid.Parse("0b7b68dc-a4c4-437f-abf7-6e00ad61ebd3"),
                DueDateTime = dueDateTime,
                Title = "Task created via POST"
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/v1/Tasks");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        // [Test]
        // public async Task Given_ExistentBoard_WhenCreateTask_ShouldCreateTask()
        // {
        //     var task = new CreateTaskModel
        //     {
        //         Description = "Task created via POST",
        //         BoardId = Guid.NewGuid(),
        //         DueDateTime = dueDateTime,
        //         Title = "Task created via POST"
        //     };
        //
        //     // Act
        //     var requestContent = new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");
        //     var request = new HttpRequestMessage(HttpMethod.Post, "api/v1/Tasks");
        //     request.Headers.Add("RequestedBy", "integration_test@email.com");
        //     request.Content = requestContent;
        //     var response = await testClient.SendAsync(request);
        //
        //     response.StatusCode.Should().Be(HttpStatusCode.Created);
        //
        //     var responseContent = await response.Content.ReadAsStringAsync();
        //     var createdTask = JsonSerializer.Deserialize<TaskDto>(
        //         responseContent,
        //         new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //
        //     createdTask.Should().NotBeNull();
        //     createdTask.Title.Should().Be("Task created via POST");
        //     createdTask.Description.Should().Be("Task created via POST");
        //     createdTask.UpdatedBy.Should().Be("integration_test@email.com");
        //     createdTask.CreatedBy.Should().Be("integration_test@email.com");
        // }
    }
}
