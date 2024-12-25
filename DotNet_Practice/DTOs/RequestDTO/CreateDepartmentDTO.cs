using DotNet_Practice.DTOs.ResponseDTO;

namespace DotNet_Practice.DTOs.RequestDTO
{
    /* L – Liskov Substitution Principal (LSP):
     * The child  CreateDepartmentDTO can completely replace its parent DepartmentDTO
     * it provides all the functionalities of DepartmentDTO*/
    public class CreateDepartmentDTO : DepartmentDTO      
    {
        public Guid Id { get; set; }
    }
}
