using System.ComponentModel.DataAnnotations;

namespace DotNet_Practice.Models
{
    public class Department
    {
        [Key]
        public Guid Id { get; set; }

        
        public string DepartmentName { get; set; }
        public string DepartmenrDescription { get; set; }

        public virtual ICollection<Student> Student { get; set; } = null!;
    }
}
