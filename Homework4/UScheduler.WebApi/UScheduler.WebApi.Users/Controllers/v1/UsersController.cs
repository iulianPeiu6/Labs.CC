using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using UScheduler.WebApi.Users.Data.Entities;
using UScheduler.WebApi.Users.Interfaces;
using UScheduler.WebApi.Users.Models;
using UScheduler.WebApi.Users.Statics;

namespace UScheduler.WebApi.Users.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> logger;
        private readonly IUsersService provider;

        public UsersController(ILogger<UsersController> logger, IUsersService provider)
        {
            this.logger = logger;
            this.provider = provider;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            logger.LogDebug("Handeling GET request on api/v1/Users");

            var result = await provider.GetAllUsersAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Users);
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = result.ErrorMessage });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            logger.LogDebug("Handeling GET request on api/v1/Users/{id}", id);

            var result = await provider.GetUserByIdAsync(id);

            if (result.IsSuccess)
            {
                return Ok(result.User);
            }

            return NotFound(new { message = result.ErrorMessage });
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel user)
        {
            logger.LogDebug("Handeling POST request on api/v1/Users");
            var result = await provider.CreateUserAsync(user);

            if (result.IsSuccess)
            {
                return Created(Request.Host.Value + $"/api/v1/Users/{result.User?.Id}", result.User);
            }

            if (result.ErrorMessage == ErrorMessage.EmailIsAlreadyUsed || result.ErrorMessage == ErrorMessage.UserNameIsAlreadyUsed)
            {
                return Conflict(new { message = result.ErrorMessage });
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserModel user)
        {
            logger.LogDebug("Handeling PUT request on api/v1/Users/{id}", id);
            var result = await provider.FullyUpdateUserAsync(id, user);

            if (result.IsSuccess)
            {
                return Ok(result.User);
            }

            if (result.ErrorMessage == ErrorMessage.UserNotFound)
            {
                return NotFound(new { message = result.ErrorMessage });
            }

            if (result.ErrorMessage == ErrorMessage.EmailIsAlreadyUsed || result.ErrorMessage == ErrorMessage.UserNameIsAlreadyUsed)
            {
                return Conflict(new { message = result.ErrorMessage });
            }

            return BadRequest(new { message = result.ErrorMessage });
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] JsonPatchDocument<User> patchDoc)
        {
            logger.LogDebug("Handeling PATCH request on api/v1/Users/{id}", id);

            if (patchDoc != null)
            {
                var result = await provider.PartiallyUpdateUserAsync(id, patchDoc);

                if (result.IsSuccess)
                {
                    return Ok(result.User);
                }

                if (result.ErrorMessage == ErrorMessage.UserNotFound)
                {
                    return NotFound(new { message = result.ErrorMessage });
                }

                if (result.ErrorMessage == ErrorMessage.EmailIsAlreadyUsed || result.ErrorMessage == ErrorMessage.UserNameIsAlreadyUsed)
                {
                    return Conflict(new { message = result.ErrorMessage });
                }

                return BadRequest(new { message = result.ErrorMessage });
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            logger.LogDebug("Handeling DELETE request on api/v1/Users/{id}", id);

            var result = await provider.DeleteUserByIdAsync(id);

            if (result.IsSuccess)
            {
                return Ok(new { message = "User deleted successfully!" });
            }

            if (result.ErrorMessage == ErrorMessage.UserNotFound)
            {
                return NotFound(new { message = result.ErrorMessage });
            }

            return StatusCode(StatusCodes.Status500InternalServerError, new { message = result.ErrorMessage });
        }
    }
}
