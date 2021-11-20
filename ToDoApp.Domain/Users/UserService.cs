using System;
using System.Threading.Tasks;
using ToDoApp.Domain.Shared;

namespace ToDoApp.Domain.Users
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> Register(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return Result.Failure("Der Benutzername ist leer.");
            }

            var user = await _userRepository.FindByName(userName);
            if (user != null)
            {
                return Result.Failure($"Es gibt bereits einen Benutzer mit dem Namen '{userName}'.");
            }
            user = new User(new UserId(Guid.NewGuid()), userName);
            await _userRepository.CreateNew(user);
            return Result.Success();
        }

        public async Task<User> FindByName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return null;
            }

            var user = await _userRepository.FindByName(userName);
            return user;
        }
    }
}
