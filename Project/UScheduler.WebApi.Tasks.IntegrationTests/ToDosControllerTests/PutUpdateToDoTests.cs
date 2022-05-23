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

namespace UScheduler.WebApi.Tasks.IntegrationTests.ToDosControllerTests
{
    public class PutUpdateToDoTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidToDo_When_PutUpdateToDoIsCalled_Then_ReturnOkWithToDo()
        {
            var newDueDate = DateTime.UtcNow.AddDays(7);
            var todo = new UpdateToDoModel()
            {
                Description = "ToDo description - 001 - Updated",
                Completed = true
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(todo), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/v1/ToDos/c073dc23-8fe1-4751-9dd7-5dadcc76f8c8");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedToDo = JsonSerializer.Deserialize<ToDoDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            updatedToDo.Should().NotBeNull();
            updatedToDo.Description.Should().Be("ToDo description - 001 - Updated");
            updatedToDo.UpdatedBy.Should().Be("integration_test@email.com");
            updatedToDo.Completed.Should().BeTrue();
        }

        [Test]
        public async Task Given_InvalidToDo_When_PutUpdateToDoIsCalled_Then_ReturnNotFound()
        {
            var task = new UpdateToDoModel();

            var requestContent = new StringContent(JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Put, $"api/v1/ToDos/{Guid.NewGuid()}");
            request.Content = requestContent;

            var response = await testClient.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
