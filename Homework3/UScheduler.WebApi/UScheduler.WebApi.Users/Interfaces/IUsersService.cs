using Microsoft.AspNetCore.JsonPatch;
using UScheduler.WebApi.Users.Data.Entities;
using UScheduler.WebApi.Users.Models;

namespace UScheduler.WebApi.Users.Interfaces
{
    public interface IUsersService
    {
        Task<(bool IsSuccess, DisplayUserModel? User, string ErrorMessage)> CreateUserAsync(CreateUserModel createUserModel);
        Task<(bool IsSuccess, string ErrorMessage)> DeleteUserByIdAsync(Guid id);
        Task<(bool IsSuccess, IEnumerable<DisplayUserModel>? Users, string ErrorMessage)> GetAllUsersAsync();
        Task<(bool IsSuccess, DisplayUserModel? User, string ErrorMessage)> GetUserByIdAsync(Guid id);
        Task<(bool IsSuccess, DisplayUserModel? User, string ErrorMessage)> FullyUpdateUserAsync(Guid id, UpdateUserModel updateUserModel);
        Task<(bool IsSuccess, DisplayUserModel? User, string ErrorMessage)> PartiallyUpdateUserAsync(Guid id, JsonPatchDocument<User> patchDoc);
    }
}