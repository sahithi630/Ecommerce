using UserServices.Models;

namespace UserServices.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByEmail(string email);
        Task AddUser(User user);
    }
}
