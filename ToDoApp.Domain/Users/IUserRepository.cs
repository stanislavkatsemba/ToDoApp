using System.Threading.Tasks;

namespace ToDoApp.Domain.Users
{
    public interface IUserRepository
    {
        Task CreateNew(User user);

        Task<User> FindBy(UserId id);
    }
}
