using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using UScheduler.WebApi.Users.Data;
using UScheduler.WebApi.Users.Data.Entities;
using UScheduler.WebApi.Users.Interfaces;
using UScheduler.WebApi.Users.Models;
using UScheduler.WebApi.Users.Statics;
using Vanguard;

namespace UScheduler.WebApi.Users.Services
{
    public class DataValidator : IDataValidator
    {
        private readonly IUserRepository repository;

        public DataValidator(IUserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<(bool Success, string Error)> ValidateCreateUserModelAsync(CreateUserModel createUserModel)
        {
            ValidateFormat(createUserModel);

            var emailIsTaken = repository
                .Query(u => u.Email == createUserModel.Email)
                .Count() > 0;

            if (emailIsTaken)
            {
                return (false, ErrorMessage.EmailIsAlreadyUsed);
            }

            var userNameIsTaken = repository
                .Query(u => u.UserName == createUserModel.UserName)
                .Count() > 0;

            if (userNameIsTaken)
            {
                return (false, ErrorMessage.UserNameIsAlreadyUsed);
            }

            return (true, string.Empty);
        }

        public async Task<(bool Success, string Error)> ValidateFullUpdateUserModel(Guid id, UpdateUserModel updateUserModel)
        {
            ValidateFormat(updateUserModel);

            var userExists = repository
                .GetById(id) != null;

            if (!userExists)
            {
                return (false, ErrorMessage.UserNotFound);
            }

            var emailIsTaken = repository
                .Query(u => u.Email == updateUserModel.Email)
                .Count() > 0;

            if (emailIsTaken)
            {
                return (false, ErrorMessage.EmailIsAlreadyUsed);
            }

            var userNameIsTaken = repository
                .Query(u => u.UserName == updateUserModel.UserName)
                .Count() > 0;

            if (userNameIsTaken)
            {
                return (false, ErrorMessage.UserNameIsAlreadyUsed);
            }

            return (true, string.Empty);
        }

        public async Task<(bool Success, string Error)> ValidateUser(Guid id, User user)
        {
            ValidateFormat(user);

            var emailIsTaken = repository
                .Query(u => u.Email == user.Email && u.Id != id)
                .Count() > 0;

            if (emailIsTaken)
            {
                return (false, ErrorMessage.EmailIsAlreadyUsed);
            }

            var userNameIsTaken =repository
                .Query(u => u.UserName == user.UserName && u.Id != id)
                .Count() > 0;

            if (userNameIsTaken)
            {
                return (false, ErrorMessage.UserNameIsAlreadyUsed);
            }

            return (true, string.Empty);
        }

        private void ValidateFormat(CreateUserModel user)
        {
            Guard.ArgumentNotNullOrEmpty(user.UserName, nameof(user.UserName), ErrorMessage.UserNameIsRequired);
            Guard.ArgumentNotNullOrEmpty(user.Email, nameof(user.Email), ErrorMessage.EmailIsRequired);
            Guard.ArgumentNotNullOrEmpty(user.HashedPassword, nameof(user.HashedPassword), ErrorMessage.PasswordIsRequired);

            if (!EmailIsValid(user.Email))
            {
                throw new ArgumentException(ErrorMessage.EmailIsInvalid, nameof(user.Email));
            }
        }

        private void ValidateFormat(User user)
        {
            Guard.ArgumentNotNullOrEmpty(user.UserName, nameof(user.UserName), ErrorMessage.UserNameIsRequired);
            Guard.ArgumentNotNullOrEmpty(user.Email, nameof(user.Email), ErrorMessage.EmailIsRequired);
            Guard.ArgumentNotNullOrEmpty(user.HashedPassword, nameof(user.HashedPassword), ErrorMessage.PasswordIsRequired);

            if (!EmailIsValid(user.Email))
            {
                throw new ArgumentException(ErrorMessage.EmailIsInvalid, nameof(user.Email));
            }
        }

        private void ValidateFormat(UpdateUserModel user)
        {
            Guard.ArgumentNotNull(user, nameof(user));
            Guard.ArgumentNotNullOrEmpty(user.UserName, nameof(user.UserName), ErrorMessage.UserNameIsRequired);
            Guard.ArgumentNotNullOrEmpty(user.Email, nameof(user.Email), ErrorMessage.EmailIsRequired);
            Guard.ArgumentNotNullOrEmpty(user.HashedPassword, nameof(user.HashedPassword), ErrorMessage.PasswordIsRequired);
            
            if (!EmailIsValid(user.Email))
            {
                throw new ArgumentException(ErrorMessage.EmailIsInvalid, nameof(user.Email));
            }
        }

        public bool EmailIsValid(string email)
        {
            var trimmedEmail = email.Trim();

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
    }
}
