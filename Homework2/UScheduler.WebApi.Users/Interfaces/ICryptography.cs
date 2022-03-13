namespace UScheduler.WebApi.Users.Interfaces
{
    public interface ICryptography
    {
        string GetPasswordSHA3Hash(string password);
    }
}