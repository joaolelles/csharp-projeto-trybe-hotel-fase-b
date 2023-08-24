using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            throw new NotImplementedException();
        }
        public UserDto Add(UserDtoInsert user)
        {
            User newUser = new User()
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client",
            };
            _context.Users.Add(newUser);
            _context.SaveChanges();
            var postUser = new UserDto
            {
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                UserType = newUser.UserType,
            };
            return postUser;
        }

        public UserDto GetUserByEmail(string userEmail)
        {
            UserDto? userByEmail = _context.Users.Where(user => user.Email == userEmail)
            .Select(user => new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            }).First();

            if (userByEmail == null)
            {
                return null!;
            }
            return userByEmail;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            throw new NotImplementedException();
        }

    }
}