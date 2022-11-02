using Data.Models;

namespace Data
{
    public class DataSeeder
    {
        private UsersContext _usersContext;

        public DataSeeder(UsersContext usersContext)
        {
            _usersContext = usersContext;
        }

        public void Seed()
        {
            if (_usersContext.Users.Any()) return;

            var user = new User()
            {
                Username = "TestUser",
                FirstName = "Test",
                LastName = "User",
                Email = "testuser@mail.com",
                Status = User.UserStatus.Active,
                Password = "Password"
            };

            user.Permissions = new List<Permission>() {
                new Permission() {
                    Name = "Code"
                },
                 new Permission()
                {
                    Name = "Description"
                }
            };

            _usersContext.Users.Add(user);
            _usersContext.SaveChanges();
        }
    }
}
