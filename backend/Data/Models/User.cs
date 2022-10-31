using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class User: BaseModel
    {
        public enum UserStatus
        {
            Inactive,
            Active
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }

        public ICollection<Permission> Permissions { get; set; }
    }
}
