using AutoMapper;
using Core.Dto;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Services
{
    public class PermissionsService
    {
        private UsersContext _usersContext;
        private readonly IMapper _mapper;

        public PermissionsService(UsersContext usersContext, IMapper mapper)
        {
            _usersContext = usersContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionDto>> GetAllPermissions()
        {
            return await _usersContext.Permissions.Select(p => _mapper.Map<PermissionDto>(p)).ToListAsync();
        }

        public async Task<IEnumerable<PermissionDto>?> GetUserPermissions(long userId)
        {
            var user = await _usersContext.Users.Include(user => user.Permissions).FirstOrDefaultAsync(s => s.UserId == userId);

            if (user == null)
            {
                return null;
            }

            return user.Permissions.Select(p => _mapper.Map<PermissionDto>(p));
        }

        public async Task<PermissionDto?> AddNewPermission(PermissionDto permission)
        {
            var _permission = await _usersContext.Permissions.FirstOrDefaultAsync(s => s.Name == permission.Name);

            if (_permission != null)
            {
                return null;
            }

            var newPermission = _mapper.Map<Permission>(permission);

            await _usersContext.Permissions.AddAsync(newPermission);
            await _usersContext.SaveChangesAsync();

            return _mapper.Map<PermissionDto>(permission);
        }

        public async Task<bool> DeletePermission(long id)
        {
            var permission = await _usersContext.Permissions.FirstOrDefaultAsync(p => p.PermissionId == id);

            if (permission == null)
            {
                return false;
            }

            _usersContext.Permissions.Remove(permission);
            await _usersContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AssignUserPermission(long userId, long permissionId)
        {
            var permission = await _usersContext.Permissions.FirstOrDefaultAsync(p => p.PermissionId == permissionId);

            if (permission == null)
            {
                return false;
            }

            var user = await _usersContext.Users.Include(user => user.Permissions).FirstOrDefaultAsync(s => s.UserId == userId);

            if (user == null)
            {
                return false;
            }

            user.Permissions.Add(permission);

            _usersContext.Entry(user).CurrentValues.SetValues(user);
            await _usersContext.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> RevokeUserPermission(long userId, long permissionId)
        {
            var user = await _usersContext.Users.Include(user => user.Permissions).FirstOrDefaultAsync(s => s.UserId == userId);

            if (user == null)
            {
                return false;
            }

            var permission = user.Permissions.FirstOrDefault(p => p.PermissionId == permissionId);

            if (permission == null)
            {
                return false;
            }

            user.Permissions.Remove(permission);
            await _usersContext.SaveChangesAsync();

            return true;
        }
    }
}
