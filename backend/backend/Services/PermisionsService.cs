using AutoMapper;
using Data;

namespace backend.Services
{
    public class PermisionsService
    {
        private UsersContext _usersContext;

        public PermisionsService(UsersContext usersContext)
        {
            _usersContext = usersContext;
        }

        public void GetAllPermisions()
        {
            return _usersContext.Permissions;
        }

        public void GetUserPermisions(int userId)
        {

        }

        public void AddNewPermision()
        {

        }

        public void DeletePermision(int id)
        {

        }

        public void AssignUserPermision(int userId, int permisionId)
        {

        }
    }
}
