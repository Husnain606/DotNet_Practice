using DotNet_Practice.DTOs.NewFolder;
using DotNet_Practice.DTOs.ResponseDTO;
using DotNet_Practice.ViewModels;

namespace DotNet_Practice.Services.Students
{
    public interface IStudentService
    {
        Task<List<StudentDTO>> GetStudentListAsync();
        Task<StudentDTO> GetStudentDetailsByIdAsync(Guid StudentID);
        Task<ResponseModel> UpdateStudentAsync(CreateStudentDTO StudentModel);
        Task<ResponseModel> CreateStudentAsync(CreateStudentDTO StudentModel);
        Task<ResponseModel> DeleteStudentAsync(Guid StudentID);
        Task<List<StudentDTO>> GetStudentDetailsByAgeG13Async(int age);    // 2.	O  - Open Closed Principal (OCP):
                                                                           // it is open for extension that it can get the student
                                                                           // by some parameter chechk  now it is depend upon service
                                                                           // that in parameter through which filter it can process

    }
}
