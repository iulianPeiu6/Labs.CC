using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using UScheduler.WebApi.Users.Data;
using UScheduler.WebApi.Users.Data.Entities;
using UScheduler.WebApi.Users.Interfaces;
using UScheduler.WebApi.Users.Models;
using UScheduler.WebApi.Users.Statics;

namespace UScheduler.WebApi.Users.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUserRepository repository;
        private readonly ILogger<UsersService> logger;
        private readonly IMapper mapper;
        private readonly IDataValidator validator;

        public UsersService(
            IUserRepository repository,
            ILogger<UsersService> logger,
            IMapper mapper,
            IDataValidator validator)
        {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<(bool IsSuccess, IEnumerable<DisplayUserModel>? Users, string ErrorMessage)> GetAllUsersAsync()
        {
            try
            {
                logger?.LogDebug("Qerying all users from database");

                var users = repository.GetAll().ToList();

                logger?.LogInformation($"{users.Count} user(s) found in database");
                var result = mapper.Map<IEnumerable<User>, IEnumerable<DisplayUserModel>>(users);
                return (true, result, string.Empty);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, DisplayUserModel? User, string ErrorMessage)> GetUserByIdAsync(Guid id)
        {
            try
            {
                logger?.LogDebug($"Qerying user by id '{id}' from database");

                var user = repository.GetById(id);

                if (user == null)
                {
                    return (false, null, ErrorMessage.UserNotFound);
                }
                logger?.LogInformation($"User with id '{id}'found in database");
                var result = mapper.Map<User, DisplayUserModel>(user);
                return (true, result, string.Empty);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, DisplayUserModel? User, string ErrorMessage)> CreateUserAsync(CreateUserModel createUserModel)
        {
            try
            {
                logger?.LogDebug($"Creating user in database");

                var validationResult = await validator.ValidateCreateUserModelAsync(createUserModel);

                if (!validationResult.Success)
                {
                    return (false, null, validationResult.Error);
                }

                var user = mapper.Map<CreateUserModel, User>(createUserModel);
                user.RegistrationDate = DateTime.UtcNow;
                user.HashedPassword = createUserModel.HashedPassword;
                user.AccountSettings = new AccountSettings
                {
                    EmailForNotification = createUserModel.Email,
                    SendNotificationOnEmail = false
                };
                var createdUser = repository.Add(user);
                var result = mapper.Map<User, DisplayUserModel>(createdUser);

                return (true, result, string.Empty);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, DisplayUserModel? User, string ErrorMessage)> FullyUpdateUserAsync(Guid id, UpdateUserModel updateUserModel)
        {
            try
            {
                logger?.LogDebug($"Fully updating user with id {id} in database");

                var validatedResult = await validator.ValidateFullUpdateUserModel(id, updateUserModel);

                if (!validatedResult.Success)
                {
                    return (false, null, validatedResult.Error);
                }

                var user = mapper.Map<UpdateUserModel, User>(updateUserModel);
                user.Id = id;
                user.HashedPassword = updateUserModel.HashedPassword;
                var createdUser = repository.Update(id, user);
                var result = mapper.Map<User, DisplayUserModel>(createdUser);
                return (true, result, string.Empty);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error while fully updating user");
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, DisplayUserModel? User, string ErrorMessage)> PartiallyUpdateUserAsync(Guid id, JsonPatchDocument<User> patchDoc)
        {
            try
            {
                logger?.LogDebug($"Partially updating user with id {id} in database");

                var user = repository.GetById(id);

                if (user == null)
                {
                    return (false, null, ErrorMessage.UserNotFound);
                }

                patchDoc.ApplyTo(user);

                var validatedResult = await validator.ValidateUser(id, user);

                if (!validatedResult.Success)
                {
                    return (false, null, validatedResult.Error);
                }

                repository.Update(id, user);
                var result = mapper.Map<User, DisplayUserModel>(user);
                return (true, result, string.Empty);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error while partially updating user");
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string ErrorMessage)> DeleteUserByIdAsync(Guid id)
        {
            try
            {
                logger?.LogDebug($"Delete user by id '{id}' from database");
                var user = repository.GetById(id);
                if (user == null)
                {
                    return (false, ErrorMessage.UserNotFound);
                }
                repository.Delete(id);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error while deleting user");
                return (false, ex.Message);
            }
        }
    }
}
