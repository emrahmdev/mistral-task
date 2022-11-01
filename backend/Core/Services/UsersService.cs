using AutoMapper;
using Core.Dto;
using Core.Models;
using Core.Utils;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Data.Models.User;

namespace Core.Services
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

        public async Task<PaginatedResponse<UserDto>> GetUsers(int page = 1, int perPage = 10, string orderBy = "", string orderType = "", string filterBy = "", string filterString = "")
        {
            var query = _usersContext.Users.AsQueryable();

            OrderUsersList(ref query, orderBy, orderType);

            FilterUsersList(ref query, filterBy, filterString);

            var users = await PaginatedList<UserDto>.CreateAsync(query, page, perPage, user => _mapper.Map<UserDto>(user));

            return _mapper.Map<PaginatedResponse<UserDto>>(users);
        }

        public UserDto? GetUserById(int id)
        {
            var user = _usersContext.Users.FirstOrDefault(s => s.UserId == id);

            return _mapper.Map<UserDto>(user);
        }

        public (UserEntryResponse, UserDto?) CreateUser(UserWithPasswordDto user)
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

            return (UserEntryResponse.Created, _mapper.Map<UserDto>(newUser));
        }

       public UserEntryResponse DeleteUser(int id)
        {

            var user = _usersContext.Users.FirstOrDefault(s => s.UserId == id);

            if (user == null)
            {
                return UserEntryResponse.NotFound;
            }

            _usersContext.Users.Remove(user);
            _usersContext.SaveChanges();

            return UserEntryResponse.Deleted;
        }
        
       public UserEntryResponse UpdateUser(int id, UserDto value)
       {
            var user = _usersContext.Users.FirstOrDefault(s => s.UserId == id);

            if (user == null)
            {
                return UserEntryResponse.NotFound;
            }

            var _user = _usersContext.Users.FirstOrDefault(s => s.Email == value.Email && s.UserId != id);

            if (_user != null)
            {
                return UserEntryResponse.EmailExists;
            }

            user.FirstName = value.FirstName;
            user.LastName = value.LastName;
            user.Email = value.Email;
            user.Status = value.Status;

           _usersContext.SaveChanges();

           return UserEntryResponse.Updated;
       }

        private void OrderUsersList(ref IQueryable<User> query, string orderBy, string orderType)
        {
            if (string.IsNullOrEmpty(orderBy))
            {
                return;
            }

            if(orderBy == "status")
            {
                query = query.OrderBy(u => u.Status);
                return;
            }

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

        private void FilterUsersList(ref IQueryable<User> query, string filterBy, string filterString)
        {
            if (string.IsNullOrEmpty(filterBy))
            {
                return;
            }

            if (filterBy == "status")
            {
                var status = (UserStatus)int.Parse(filterString);

                query = query.Where(u => u.Status == status);
                return;
            }

            var filter = UsersFilter(filterBy, filterString);

            if (filter != null)
            {
                query = query.Where(filter);
            }
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
                "all" => (u) => u.FirstName.Contains(filter) || u.LastName.Contains(filter) || u.Username.Contains(filter) || u.Email.Contains(filter),
                _ => null
            };
        }
    }
}
