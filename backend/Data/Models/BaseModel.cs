
namespace Data.Models
{
    public abstract class BaseModel
    {
        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

    }
}
