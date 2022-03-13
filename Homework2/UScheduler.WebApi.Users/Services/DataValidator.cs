using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using UScheduler.WebApi.Users.Data;
using UScheduler.WebApi.Users.Interfaces;
using UScheduler.WebApi.Users.Models;
using UScheduler.WebApi.Users.Statics;
using Vanguard;

namespace UScheduler.WebApi.Users.Services
{
    public class DataValidator : IDataValidator
    {
        private readonly USchedulerContext context;

        public DataValidator(USchedulerContext context)
        {
            this.context = context;
        }

        public bool EmailIsvalid(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }

            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public async Task<(bool Success, string Error)> ValidateCreateUserModelAsync(CreateUserModel createUserModel)
        {
            Guard.ArgumentNotNullOrEmpty(createUserModel.UserName, nameof(createUserModel.UserName), ErrorMessage.UserNameIsRequired);
            Guard.ArgumentNotNullOrEmpty(createUserModel.Email, nameof(createUserModel.Email), ErrorMessage.EmailIsRequired);

            if (!EmailIsvalid(createUserModel.Email))
            {
                throw new ArgumentException(ErrorMessage.EmailIsInvalid, nameof(createUserModel.Email));
            }

            Guard.ArgumentNotNullOrEmpty(createUserModel.Password, nameof(createUserModel.Password), ErrorMessage.PasswordIsRequired);

            var emailIsTaken = await context.Users
                .Where(u => u.Email == createUserModel.Email)
                .AnyAsync();

            if (emailIsTaken)
            {
                return (false, ErrorMessage.EmailIsAlreadyUsed);
            }

            var userNameIsTaken = await context.Users
                .Where(u => u.UserName == createUserModel.UserName)
                .AnyAsync();

            if (userNameIsTaken)
            {
                return (false, ErrorMessage.UserNameIsAlreadyUsed);
            }

            return (true, null);
        }

        public async Task<(bool Success, string Error)> ValidateFullUpdateUserModel(Guid id, UpdateUserModel updateUserModel)
        {
            Guard.ArgumentNotNull(updateUserModel, nameof(updateUserModel));

            if (!EmailIsvalid(updateUserModel.Email))
            {
                throw new ArgumentException(ErrorMessage.EmailIsInvalid, nameof(updateUserModel.Email));
            }

            Guard.ArgumentNotNullOrEmpty(updateUserModel.Password, nameof(updateUserModel.Password), ErrorMessage.PasswordIsRequired);

            var userExists = await context.Users
                .Where(u => u.Id == id)
                .AnyAsync();

            if (!userExists)
            {
                return (false, ErrorMessage.UserNotFound);
            }

            var emailIsTaken = await context.Users
                .Where(u => u.Email == updateUserModel.Email && u.Id != id)
                .AnyAsync();

            if (emailIsTaken)
            {
                return (false, ErrorMessage.EmailIsAlreadyUsed);
            }

            var userNameIsTaken = await context.Users
                .Where(u => u.UserName == updateUserModel.UserName && u.Id != id)
                .AnyAsync();

            if (userNameIsTaken)
            {
                return (false, ErrorMessage.UserNameIsAlreadyUsed);
            }

            return (true, null);
        }

        public async Task<(bool Success, string Error)> ValidatePartialUpdateUserModel(Guid id, UpdateUserModel updateUserModel)
        {
            if (updateUserModel.UserName == string.Empty)
            {
                throw new ArgumentException(ErrorMessage.UserNameIsRequired, nameof(updateUserModel.UserName));
            }

            if (updateUserModel.Email != null && !EmailIsvalid(updateUserModel.Email))
            {
                throw new ArgumentException(ErrorMessage.EmailIsInvalid, nameof(updateUserModel.Email));
            }

            if (updateUserModel.Email != null)
            {
                var emailIsTaken = await context.Users
                    .Where(u => u.Email == updateUserModel.Email && u.Id != id)
                    .AnyAsync();

                if (emailIsTaken)
                {
                    return (false, ErrorMessage.EmailIsAlreadyUsed);
                }
            }

            if (updateUserModel.AccountSettings?.EmailForNotification != null)
            {
                var emailIsTaken = await context.Users
                    .Where(u => u.Email == updateUserModel.Email && u.Id != id)
                    .AnyAsync();

                if (emailIsTaken)
                {
                    return (false, ErrorMessage.EmailIsAlreadyUsed);
                }
            }

            if (updateUserModel.UserName != null)
            {
                var userNameIsTaken = await context.Users
                    .Where(u => u.UserName == updateUserModel.UserName && u.Id != id)
                    .AnyAsync();

                if (userNameIsTaken)
                {
                    return (false, ErrorMessage.UserNameIsAlreadyUsed);
                }
            }

            return (true, null);
        }
    }
}
