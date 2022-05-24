using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using NUnit.Framework;
using UScheduler.WebApi.Tasks.Data.Entities;
using UScheduler.WebApi.Tasks.Models.Task;
using UScheduler.WebApi.Tasks.Models.ToDo;
using Task = System.Threading.Tasks.Task;

namespace UScheduler.WebApi.Tasks.IntegrationTests.TasksControllerTests
{
    internal class PatchUpdateTaskTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidToDo_When_PatchUpdateToDoIsCalled_Then_ReturnOkWithToDo()
        {
            var todo = new JsonPatchDocument<Data.Entities.Task>();

            todo.Replace(t => t.Description, "Task description - 003 patch updated");

            var requestContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json-patch+json");
            var request = new HttpRequestMessage(HttpMethod.Patch, "api/v1/Tasks/5939ca2e-e9de-4d13-ab6a-81461422cd1b");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);

            //response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedTask = System.Text.Json.JsonSerializer.Deserialize<TaskDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            updatedTask.Should().NotBeNull();
            updatedTask.Description.Should().Be("Task description - 003 patch updated");
            updatedTask.UpdatedBy.Should().Be("integration_test@email.com");
        }

        [Test]
        public async Task Given_InvalidToDo_When_PutUpdateToDoIsCalled_Then_ReturnNotFound()
        {
            var todo = new JsonPatchDocument<Data.Entities.Task>();

            todo.Replace(t => t.Description, "Task description - 003 patch updated");

            var requestContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json-patch+json");
            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/v1/Tasks/{Guid.NewGuid()}");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;

            var response = await testClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
