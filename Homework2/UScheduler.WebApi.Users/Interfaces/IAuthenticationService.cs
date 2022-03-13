namespace UScheduler.WebApi.Users.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateToken(Guid id, string username, string email);
    }
}