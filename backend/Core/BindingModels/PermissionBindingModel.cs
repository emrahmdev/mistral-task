
using System.ComponentModel.DataAnnotations;

namespace Core.BindingModels
{
    public class PermissionBindingModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
    }
}
