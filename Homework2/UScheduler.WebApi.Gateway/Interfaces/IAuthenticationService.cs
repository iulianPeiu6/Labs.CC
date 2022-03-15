namespace UScheduler.WebApi.Gateway.Interfaces
{
    public interface IAuthenticationService
    {
        string GenerateToken(Guid id, string username, string email);
    }
}