using System.Threading.Tasks;

namespace ToDoApp.Domain.Users
{
    public interface IUserRepository
    {
        Task CreateNew(User user);

        Task<User> FindById(UserId id);

        Task<User> FindByName(string name);
    }
}
