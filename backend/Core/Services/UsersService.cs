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

            query = OrderUsersList(query, orderBy, orderType);

            query = FilterUsersList(query, filterBy, filterString);

            var users = await PaginatedList<UserDto>.CreateAsync(query, page, perPage, user => _mapper.Map<UserDto>(user));

            return _mapper.Map<PaginatedResponse<UserDto>>(users);
        }

        public async Task<UserDto?> GetUserById(int id)
        {
            var user = await _usersContext.Users.FirstOrDefaultAsync(s => s.UserId == id);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<(UserEntryResponse, UserDto?)> CreateUser(UserWithPasswordDto user)
        {
            var _user = await _usersContext.Users.FirstOrDefaultAsync(s => s.Email == user.Email || s.Username == user.Username);

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

            await _usersContext.Users.AddAsync(newUser);
            await _usersContext.SaveChangesAsync();

            return (UserEntryResponse.Created, _mapper.Map<UserDto>(newUser));
        }

       public async Task<UserEntryResponse> DeleteUser(int id)
        {

            var user = await _usersContext.Users.FirstOrDefaultAsync(s => s.UserId == id);

            if (user == null)
            {
                return UserEntryResponse.NotFound;
            }

            user.IsDeleted = true;
            await _usersContext.SaveChangesAsync();

            return UserEntryResponse.Deleted;
        }
        
       public async Task<UserEntryResponse> UpdateUser(int id, UserDto value)
       {
            var user = await _usersContext.Users.FirstOrDefaultAsync(s => s.UserId == id);

            if (user == null)
            {
                return UserEntryResponse.NotFound;
            }

            var _user = await _usersContext.Users.FirstOrDefaultAsync(s => s.Email == value.Email && s.UserId != id);

            if (_user != null)
            {
                return UserEntryResponse.EmailExists;
            }

            user.FirstName = value.FirstName;
            user.LastName = value.LastName;
            user.Email = value.Email;
            user.Status = value.Status;

           await _usersContext.SaveChangesAsync();

           return UserEntryResponse.Updated;
       }

        private IQueryable<User> OrderUsersList(IQueryable<User> query, string orderBy, string orderType)
        {
            if (string.IsNullOrEmpty(orderBy) || string.IsNullOrEmpty(orderType))
            {
                return query;
            }

            if(orderBy == "status")
            {
                if (orderType == "asc")
                {
                    return query.OrderBy(u => u.Status);
                }
                else if (orderType == "desc")
                {
                    return query.OrderByDescending(u => u.Status);
                }
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

            return query;
        }

        private IQueryable<User> FilterUsersList(IQueryable<User> query, string filterBy, string filterString)
        {
            if (string.IsNullOrEmpty(filterBy) || string.IsNullOrEmpty(filterString))
            {
                return query;
            }

            if (filterBy == "status")
            {
                var status = (UserStatus)int.Parse(filterString);

                return query.Where(u => u.Status == status);
            }

            var filter = UsersFilter(filterBy, filterString);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return query;
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
