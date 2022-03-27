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
        private readonly UsersContext context;
        private readonly ILogger<UsersService> logger;
        private readonly IMapper mapper;
        private readonly IDataValidator validator;

        public UsersService(
            UsersContext context,
            ILogger<UsersService> logger,
            IMapper mapper,
            IDataValidator validator)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
            this.validator = validator;
        }

        public async Task<(bool IsSuccess, IEnumerable<DisplayUserModel>? Users, string ErrorMessage)> GetAllUsersAsync()
        {
            try
            {
                logger?.LogDebug("Qerying all users from database");

                var users = await context.Users
                    .Join(
                        context.AccountSettings,
                        user => user.AccountSettingsId,
                        settings => settings.Id,
                        (user, settings) => new User
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            RegistrationDate = user.RegistrationDate,
                            AccountSettingsId = user.AccountSettingsId,
                            AccountSettings = settings
                        })
                    .ToListAsync();

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

                var user = await context.Users
                    .Join(
                        context.AccountSettings,
                        user => user.AccountSettingsId,
                        settings => settings.Id,
                        (user, settings) => new User
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            RegistrationDate = user.RegistrationDate,
                            AccountSettingsId = user.AccountSettingsId,
                            AccountSettings = settings
                        })
                    .SingleOrDefaultAsync(u => u.Id == id);

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
                var response = await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                var createdUser = response.Entity;
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
                var response = context.Users.Update(user);
                await context.SaveChangesAsync();
                var createdUser = response.Entity;
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

                var user = await context.Users
                    .Join(
                        context.AccountSettings,
                        user => user.AccountSettingsId,
                        settings => settings.Id,
                        (user, settings) => new User
                        {
                            Id = user.Id,
                            UserName = user.UserName,
                            Email = user.Email,
                            RegistrationDate = user.RegistrationDate,
                            AccountSettingsId = user.AccountSettingsId,
                            HashedPassword = user.HashedPassword,
                            AccountSettings = settings
                        })
                    .SingleOrDefaultAsync(c => c.Id == id);

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

                context.Users.Update(user);
                await context.SaveChangesAsync();
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
                var user = await context.Users.SingleOrDefaultAsync(c => c.Id == id);
                if (user == null)
                {
                    return (false, ErrorMessage.UserNotFound);
                }
                context.Users.Remove(user);
                await context.SaveChangesAsync();
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
