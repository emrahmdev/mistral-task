using AutoMapper;
using Core.Dto;
using Data;
using Data.Models;

namespace Core.Services
{
    public class PermisionsService
    {
        private UsersContext _usersContext;
        private readonly IMapper _mapper;

        public PermisionsService(UsersContext usersContext, IMapper mapper)
        {
            _usersContext = usersContext;
            _mapper = mapper;
        }

        public IEnumerable<PermissionDto> GetAllPermisions()
        {
            return _usersContext.Permissions.ToList().Select(p => _mapper.Map<PermissionDto>(p));
        }

        public IEnumerable<PermissionDto>? GetUserPermisions(int userId)
        {
            var user = _usersContext.Users.FirstOrDefault(s => s.UserId == userId);

            if (user == null)
            {
                return null;
            }

            return user.Permissions.ToList().Select(p => _mapper.Map<PermissionDto>(p));
        }

        public PermissionDto? AddNewPermision(PermissionDto permission)
        {
            var _permission = _usersContext.Permissions.FirstOrDefault(s => s.Name == permission.Name);

            if (_permission != null)
            {
                return null;
            }

            var newPermission = _mapper.Map<Permission>(permission);

            _usersContext.Permissions.Add(newPermission);
            _usersContext.SaveChanges();

            return _mapper.Map<PermissionDto>(permission);
        }

        public bool DeletePermision(int id)
        {
            var permission = _usersContext.Permissions.FirstOrDefault(p => p.PermisionId == id);

            if (permission == null)
            {
                return false;
            }

            _usersContext.Permissions.Remove(permission);
            _usersContext.SaveChanges();

            return true;
        }

        public bool AssignUserPermision(int userId, int permisionId)
        {
            var permission = _usersContext.Permissions.FirstOrDefault(p => p.PermisionId == permisionId);

            if (permission == null)
            {
                return false;
            }

            var user = _usersContext.Users.FirstOrDefault(s => s.UserId == userId);

            if (user == null)
            {
                return false;
            }

            user.Permissions.Add(permission);

            _usersContext.Entry(user).CurrentValues.SetValues(user);
            _usersContext.SaveChanges();
            
            return true;
        }
    }
}
