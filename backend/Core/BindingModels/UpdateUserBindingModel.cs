using System.ComponentModel.DataAnnotations;
using static Data.Models.User;

namespace Core.BindingModels
{
    public class UpdateUserBindingModel
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(200)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Required]
        public UserStatus Status { get; set; }
    }
}
