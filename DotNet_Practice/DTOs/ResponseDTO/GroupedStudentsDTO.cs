namespace DotNet_Practice.DTOs.ResponseDTO
{
    public class GroupedStudentsDTO
    {
        public Guid DepartmentId { get; set; }
        public List<StudentDTO> Students { get; set; } 
    }

}

