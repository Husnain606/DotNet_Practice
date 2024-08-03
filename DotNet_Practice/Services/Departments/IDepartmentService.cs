using DotNet_Practice.DTOs.RequestDTO;
using DotNet_Practice.DTOs.ResponseDTO;
using DotNet_Practice.ViewModels;

namespace DotNet_Practice.Services.Departments
{
    public interface IDepartmentService
    {
        Task<List<DepartmentDTO>> GetDepartmentListAsync();
        Task<DepartmentDTO> GetDepartmentDetailsByIdAsync(Guid DepartmentID);
        Task<ResponseModel> UpdateDepartmentAsync(CreateDepartmentDTO DepartmentModel);
        Task<ResponseModel> CreateDepartmentAsync(CreateDepartmentDTO DepartmentModel);
        Task<ResponseModel> DeleteDepartmentAsync(Guid DepartmentID);
    }
}
