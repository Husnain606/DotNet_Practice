using System.ComponentModel.DataAnnotations;

namespace DotNet_Practice.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        
        public string DepartmentName { get; set; }
        public string DepartmenrDescription { get; set; }

        public ICollection<Student> Student { get; set; } = null;
    }
}
