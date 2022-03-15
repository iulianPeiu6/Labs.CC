using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UScheduler.WebApi.Users.Data;
using UScheduler.WebApi.Users.Data.Entities;
using UScheduler.WebApi.Users.Interfaces;
using UScheduler.WebApi.Users.Models;
using UScheduler.WebApi.Users.Statics;
using Vanguard;

namespace UScheduler.WebApi.Users.Services
{
    public class UsersService : IUsersService
    {
        private readonly USchedulerContext context;
        private readonly ILogger<UsersService> logger;
        private readonly IMapper mapper;
        private readonly IDataValidator validator;
        private readonly ICryptography cryptography;
        private readonly IAuthenticationService authentication;

        public UsersService(
            USchedulerContext context,
            ILogger<UsersService> logger,
            IMapper mapper,
            IDataValidator validator,
            ICryptography cryptography,
            IAuthenticationService authentication)
        {
            this.context = context;
            this.logger = logger;
            this.mapper = mapper;
            this.validator = validator;
            this.cryptography = cryptography;
            this.authentication = authentication;
        }

        public async Task<(bool IsSuccess, IEnumerable<DisplayUserModel> Users, string ErrorMessage)> GetAllUsersAsync()
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
                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, DisplayUserModel User, string ErrorMessage)> GetUserByIdAsync(Guid id)
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
                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, DisplayUserModel User, string JwtToken, string ErrorMessage)> AuthenticateUserAsync(AuthenticateUserModel authUser)
        {
            try
            {
                logger?.LogDebug($"Authenticating User: Username: '{authUser.UserName}', Email: '{authUser.Email}'");


                var user = await context.Users
                    .SingleOrDefaultAsync(
                        u => u.HashedPassword == cryptography.GetPasswordSHA3Hash(authUser.Password) &&
                            (u.UserName == authUser.UserName || u.Email == authUser.Email));

                if (user == null)
                {
                    return (false, null, null, ErrorMessage.UserAutheticatedFailed);
                }
                logger?.LogDebug($"User with id '{user.Id}' is authenticated");
                var result = mapper.Map<User, DisplayUserModel>(user);
                var jwtToken = authentication.GenerateToken(user.Id, user.UserName, user.Email);
                return (true, result, jwtToken, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, DisplayUserModel User, string ErrorMessage)> CreateUserAsync(CreateUserModel createUserModel)
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
                user.HashedPassword = cryptography.GetPasswordSHA3Hash(createUserModel.Password);
                user.AccountSettings = new AccountSettings
                {
                    EmailForNotification = createUserModel.Email,
                    SendNotificationOnEmail = false
                };
                var response = await context.Users.AddAsync(user);
                await context.SaveChangesAsync();
                var createdUser = response.Entity;
                var result = mapper.Map<User, DisplayUserModel>(createdUser);

                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, DisplayUserModel User, string ErrorMessage)> FullyUpdateUserAsync(Guid id, UpdateUserModel updateUserModel)
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
                user.HashedPassword = cryptography.GetPasswordSHA3Hash(updateUserModel.Password);
                var response = context.Users.Update(user);
                await context.SaveChangesAsync();
                var createdUser = response.Entity;
                var result = mapper.Map<User, DisplayUserModel>(createdUser);
                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, DisplayUserModel User, string ErrorMessage)> PartiallyUpdateUserAsync(Guid id, UpdateUserModel updateUserModel)
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

                var validatedResult = await validator.ValidatePartialUpdateUserModel(id, updateUserModel);

                if (!validatedResult.Success)
                {
                    return (false, null, validatedResult.Error);
                }

                if (updateUserModel.UserName != null)
                {
                    user.UserName = updateUserModel.UserName;
                }

                if (updateUserModel.Email != null)
                {
                    user.Email = updateUserModel.Email;
                }

                if (updateUserModel.Password != null)
                {
                    user.HashedPassword = cryptography.GetPasswordSHA3Hash(updateUserModel.Password);
                }

                if (updateUserModel.AccountSettings?.EmailForNotification != null)
                {
                    user.AccountSettings.EmailForNotification = updateUserModel.AccountSettings.EmailForNotification;
                }

                if (updateUserModel.AccountSettings?.SendNotificationOnEmail != null)
                {
                    user.AccountSettings.SendNotificationOnEmail = updateUserModel.AccountSettings.SendNotificationOnEmail;
                }

                var response = context.Users.Update(user);
                await context.SaveChangesAsync();
                var result = mapper.Map<User, DisplayUserModel>(user);
                return (true, result, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
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
                logger?.LogError($"User with id '{id}'found in database");
                return (true, null);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, ex.Message);
            }
        }
    }
}
