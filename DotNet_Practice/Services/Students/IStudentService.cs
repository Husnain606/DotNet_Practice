﻿using DotNet_Practice.DTOs.NewFolder;
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
        Task<List<StudentDTO>> GetStudentDetailsByAgeG13Async(int age);
        Task<List<StudentDTO>> InnerJoin();
        Task<List<StudentDTO>> GetLeftOuterJoinFields();
        Task<List<StudentDTO>> GetRightOuterJoinFields();
        Task<List<StudentDTO>> GetLeftInnerJoinFields();
        Task<List<StudentDTO>> GetRightInnerJoinFields();
        Task<List<StudentDTO>> GetGroupJoinFields();
        Task<List<GroupedStudentsDTO>> GroupByDepartment();
    }
}
