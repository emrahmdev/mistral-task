using AutoMapper;
using backend.Models;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace backend.Services
{
    public class UsersService
    {
        private UsersContext _usersContext;
        private readonly IMapper _mapper;

        public enum UserEntryResponse
        {
            Created,
            Deleted,
            Updated,

            NotFound,
            EmailExists,
            UsernameExists
        }

        public UsersService(UsersContext usersContext, IMapper mapper)
        {
            _usersContext = usersContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserResponse>> GetUsers(int page = 1, int perPage = 10, string orderBy = "", string orderType = "", string filterBy = "", string filterString = "")
        {
            var query = _usersContext.Users.AsQueryable();

            if (!string.IsNullOrEmpty(orderBy))
            {
                var order = UsersOrder(orderBy);

                if (orderType == "asc" && order != null)
                {
                    query = query.OrderBy(order);
                }
                else if (orderType == "desc" && order != null)
                {
                    query = query.OrderByDescending(order);
                }
            }

            if (!string.IsNullOrEmpty(filterString))
            {
                var filter = UsersFilter(filterBy, filterString);

                if (filter != null)
                {
                    query = query.Where(filter);
                }
            }

            return await query.Skip((page - 1) * perPage).Take(perPage).Select(user => _mapper.Map<UserResponse>(user)).ToListAsync();
        }

        public UserResponse? GetUserById(int id)
        {
            var user = _usersContext.Users.FirstOrDefault(s => s.UserId == id);

            return _mapper.Map<UserResponse>(user);
        }

        public (UserEntryResponse, UserResponse?) CreateUser(UserRequest user)
        {
            var _user = _usersContext.Users.FirstOrDefault(s => s.Email == user.Email || s.Username == user.Username);

            if (_user != null)
            {
                if(_user.Email == user.Email)
                {
                    return (UserEntryResponse.EmailExists, null);
                }

                if (_user.Username == user.Username)
                {
                    return (UserEntryResponse.UsernameExists, null);
                }
            }

            var newUser = _mapper.Map<User>(user);

            _usersContext.Users.Add(newUser);
            _usersContext.SaveChanges();

            return (UserEntryResponse.UsernameExists, _mapper.Map<UserResponse>(newUser));
        }

        public UserEntryResponse DeleteUser(int id)
        {
            var user = _usersContext.Users.FirstOrDefault(s => s.UserId == id);

            if (user != null)
            {
                return UserEntryResponse.NotFound;
            }

            _usersContext.Users.Remove(user);
            _usersContext.SaveChanges();

            return UserEntryResponse.Deleted;
        }

        public UserEntryResponse UpdateUser(int id, UserRequest value)
        { 
            var user = _usersContext.Users.FirstOrDefault(s => s.UserId == id);

            if (user == null)
            {
                return UserEntryResponse.NotFound;
            }

            _usersContext.Entry(user).CurrentValues.SetValues(value);
            _usersContext.SaveChanges();

            return UserEntryResponse.Updated;
        }

        private Expression<Func<User, string>>? UsersOrder(string orderBy)
        {
            return orderBy switch {
                "firstName" => (u) => u.FirstName,
                "lastName" => (u) => u.LastName,
                "username" => (u) => u.Username,
                "email" => (u) => u.Email,
                _ => null
            };
        }

        private Expression<Func<User, bool>>? UsersFilter(string filterBy, string filter)
        {
            return filterBy switch {
                "firstName" => (u) => u.FirstName.Contains(filter),
                "lastName" => (u) => u.LastName.Contains(filter),
                "username" => (u) => u.Username.Contains(filter),
                "email" => (u) => u.Email.Contains(filter),
                _ => null
            };
        }
    }
}
