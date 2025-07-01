using UsersManagement.Models;

namespace UsersManagement.Repositories
{
    public interface IUsersRepository
    {
        Task<IEnumerable<UserModel>> GetAllAsync();

    }
}
