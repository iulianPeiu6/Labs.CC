using SHA3.Net;
using System.Text;
using UScheduler.WebApi.Users.Interfaces;

namespace UScheduler.WebApi.Users.Services
{
    public class CryptographyService : ICryptography
    {
        public string GetPasswordSHA3Hash(string password)
        {
            using (var shaAlg = Sha3.Sha3256())
            {
                var hasedPasswordBytes = shaAlg.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hasedPassword = Convert.ToBase64String(hasedPasswordBytes);
                return hasedPassword;
            }
        }
    }
}
