using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UScheduler.WebApi.Tasks.Models.ToDo;

namespace UScheduler.WebApi.Tasks.IntegrationTests.TasksControllerTests
{
    internal class PostTodoTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidTask_WhenCreateToDo_Should_ReturnCreatedTodo()
        {
            var taskId = Guid.Parse("7c139c88-48d6-4233-8584-4db3389cf3e1");
            var todo = new CreateToDoModel()
            {
                Description = "Todo description - 001"
            };

            // Act
            var requestContent = new StringContent(JsonSerializer.Serialize(todo), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/v1/Tasks/{taskId}/ToDos");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var responseContent = await response.Content.ReadAsStringAsync();
            var createdToDo = JsonSerializer.Deserialize<ToDoDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            createdToDo.Should().NotBeNull();
            createdToDo.Description.Should().Be("Todo description - 001");
            createdToDo.UpdatedBy.Should().Be("integration_test@email.com");
            createdToDo.CreatedBy.Should().Be("integration_test@email.com");
            createdToDo.Completed.Should().BeFalse();
        }

        [Test]
        public async Task Given_InvalidTask_WhenCreateToDo_Should_ReturnNotFound()
        {
            var taskId = Guid.NewGuid();
            var todo = new CreateToDoModel();

            var requestContent = new StringContent(JsonSerializer.Serialize(todo), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/v1/Tasks/{taskId}/ToDos");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
