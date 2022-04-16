using UScheduler.WebApi.Users.Data.Entities;
using UScheduler.WebApi.Users.Models;

namespace UScheduler.WebApi.Users.Interfaces
{
    public interface IDataValidator
    {
        bool EmailIsValid(string email);
        Task<(bool Success, string Error)> ValidateCreateUserModelAsync(CreateUserModel createUserModel);
        Task<(bool Success, string Error)> ValidateFullUpdateUserModel(Guid id, UpdateUserModel updateUserModel);
        Task<(bool Success, string Error)> ValidateUser(Guid id, User updateUserModel);
    }
}