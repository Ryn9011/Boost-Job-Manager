using System.Threading.Tasks;
using JobTracker.API.Models;

namespace JobTracker.API.data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user, string password, string role);
         Task<User> Login(string username, string password);
         Task<bool> UserExists(string username);
    }
}