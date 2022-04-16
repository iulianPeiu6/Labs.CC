namespace UScheduler.WebApi.Users.Statics
{
    public static class ErrorMessage
    {
        public const string EmailIsInvalid = "Email is invalid!";
        public const string EmailIsRequired = "Email can not be null or empty!";
        public const string UserNameIsRequired = "UserName can not be null or empty!";
        public const string PasswordIsRequired = "Password can not be null or empty!";
        public const string UserNameIsAlreadyUsed = "UserName is already used!";
        public const string EmailIsAlreadyUsed = "Email is already used!";
        public const string UserIdIsRequired = "User Id can not be null or empty!";
        public const string UserIsRequired = "User Is Required!";
        public const string UserNotFound = "User Not Found!";
        public const string UserAutheticatedFailed = "User authentication failed!";
        public const string UnauthorizedToDeleteOthersAccounts = "Unauthorized to delete others accounts!";
    }
}
