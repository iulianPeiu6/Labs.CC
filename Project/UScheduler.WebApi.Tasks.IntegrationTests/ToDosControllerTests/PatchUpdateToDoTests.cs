using System;
using System.Net;
using System.Net.Http;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using NUnit.Framework;
using UScheduler.WebApi.Tasks.Data.Entities;
using UScheduler.WebApi.Tasks.Models.ToDo;
using Task = System.Threading.Tasks.Task;
using Newtonsoft.Json;
using JsonSerializerOptions = System.Text.Json.JsonSerializerOptions;

namespace UScheduler.WebApi.Tasks.IntegrationTests.ToDosControllerTests
{
    internal class PatchUpdateToDoTests : IntegrationTestBase
    {
        [Test]
        public async Task Given_ValidToDo_When_PatchUpdateToDoIsCalled_Then_ReturnOkWithToDo()
        {
            var todo = new JsonPatchDocument<ToDo>();

            todo.Replace(t => t.Description, "ToDo description - 002 patch updated");
            todo.Replace(t => t.Completed, true);

            var requestContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json-patch+json");
            var request = new HttpRequestMessage(HttpMethod.Patch, "api/v1/ToDos/c073dc23-8fe1-4751-9dd7-5dadcc76f8c8");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;
            var response = await testClient.SendAsync(request);

            //response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync();
            var updatedToDo = System.Text.Json.JsonSerializer.Deserialize<ToDoDto>(
                responseContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            updatedToDo.Should().NotBeNull();
            updatedToDo.Description.Should().Be("ToDo description - 002 patch updated");
            updatedToDo.UpdatedBy.Should().Be("integration_test@email.com");
            updatedToDo.Completed.Should().BeTrue();
        }

        [Test]
        public async Task Given_InvalidToDo_When_PutUpdateToDoIsCalled_Then_ReturnNotFound()
        {
            var todo = new JsonPatchDocument<ToDo>();

            todo.Replace(t => t.Description, "ToDo description - 003 patch updated");
            todo.Replace(t => t.Completed, true);

            var requestContent = new StringContent(JsonConvert.SerializeObject(todo), Encoding.UTF8, "application/json-patch+json");
            var request = new HttpRequestMessage(HttpMethod.Patch, $"api/v1/ToDos/{Guid.NewGuid()}");
            request.Headers.Add("RequestedBy", "integration_test@email.com");
            request.Content = requestContent;

            var response = await testClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
