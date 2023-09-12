using TrybeHotel.Models;
using TrybeHotel.Dto;
using System.ComponentModel.DataAnnotations;

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
            User? userByLogin = _context.Users.FirstOrDefault(user => user.Email == login.Email
            && user.Password == login.Password);

            if (userByLogin == null)
            {
                return null!;
            }
            return new UserDto
            {
                UserId = userByLogin!.UserId,
                Name = userByLogin.Name,
                Email = userByLogin.Email,
                UserType = userByLogin.UserType,
            };
        }
        public UserDto Add(UserDtoInsert user)
        {
            var newUser = new User()
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
            }).FirstOrDefault();

            if (userByEmail == null)
            {
                return null!;
            }
            return userByEmail;
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var user = _context.Users.Select(user => new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                UserType = user.UserType,
            });
            return user;
        }

    }
}