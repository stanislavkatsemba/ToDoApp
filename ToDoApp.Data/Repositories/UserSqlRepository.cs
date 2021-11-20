using System.Threading.Tasks;
using ToDoApp.Data.Models;
using ToDoApp.Domain.Users;

namespace ToDoApp.Data.Repositories
{
    public class UserSqlRepository : IUserRepository
    {
        private readonly ToDoAppContext _databaseContext;

        public UserSqlRepository(ToDoAppContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task CreateNew(User user)
        {
            await _databaseContext.Users.AddAsync(new UserModel(user.ToSnapshot()));
            await _databaseContext.SaveChangesAsync();
        }

        public async Task<User> FindBy(UserId id)
        {
            var snapshot = await _databaseContext.Users.FindAsync(id.Value);
            return snapshot != null ? User.FromSnapshot(snapshot) : null;
        }
    }
}
