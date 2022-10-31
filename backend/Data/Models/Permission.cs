using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Permission : BaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PermisionId { get; set; }
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
