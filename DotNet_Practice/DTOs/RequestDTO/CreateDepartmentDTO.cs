using DotNet_Practice.DTOs.ResponseDTO;

namespace DotNet_Practice.DTOs.RequestDTO
{
    public class CreateDepartmentDTO : DepartmentDTO
    {
        public Guid Id { get; set; }
    }
}
