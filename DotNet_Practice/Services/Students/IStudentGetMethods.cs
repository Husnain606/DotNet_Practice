using DotNet_Practice.DTOs.ResponseDTO;

namespace DotNet_Practice.Services.Students
{
    public interface IStudentGetMethods
    {
        /* 	I  - Interface Segregation Principle (ISP):
         * Create IStudentGetMethods interface rather then
         * put all the joins and groups method declaration
         * in single interface IStudentServices*/
        Task<List<StudentDTO>> InnerJoin();
        Task<List<StudentDTO>> GetLeftOuterJoinFields();
        Task<List<StudentDTO>> GetRightOuterJoinFields();
        Task<List<StudentDTO>> GetLeftInnerJoinFields();
        Task<List<StudentDTO>> GetRightInnerJoinFields();
        Task<List<StudentDTO>> GetGroupJoinFields();
        Task<List<GroupedStudentsDTO>> GroupByDepartment();
    }
}
