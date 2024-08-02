using DotNet_Practice.ViewModels;
using DotNet_Practice.Repository;
using DotNet_Practice.Models;
using AutoMapper;
using DotNet_Practice.Controllers.StdController;
using DotNet_Practice.DTOs.ResponseDTO;
using DotNet_Practice.DTOs.NewFolder;
using Microsoft.EntityFrameworkCore;



namespace DotNet_Practice.Services
{
    public class StudentService : IStudentService
    {

        private readonly ISchoolRepository<Student> _studentService;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentController> _logger;
    


        public StudentService(ISchoolRepository<Student> service, IMapper mapper, ILogger<StudentController> logger)
        {
            _studentService = service;
            _mapper = mapper;
            _logger = logger;
           
        }

        // CREATE STUDENT
        public async Task<ResponseModel> CreateStudentAsync(CreateStudentDTO StudentModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var student = _mapper.Map<Student>(StudentModel);

                model = await _studentService.CreateAsync(student);
                model.IsSuccess = true;
                model.Messsage = "Student Created Successfully";
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error: " + ex.Message;
            }
            return model;
        }

        // GET THE LIST OF ALL STUDENTS
        public async Task<List<StudentDTO>> GetStudentListAsync()
        {
            try
            {
                _logger.LogInformation("Getting all the students executed !!");
                var students = await _studentService.GetAllAsync();
                if (students == null) return null;
                var studentDTOs = _mapper.Map<List<StudentDTO>>(students);
                return studentDTOs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET STUDENT DETAILS BY STUDENT ID
        public async Task<StudentDTO> GetStudentDetailsByIdAsync(Guid StudentID)
        {
            Student std;
            try
            {
                var student = await _studentService.GetByIdAsync(StudentID);
                if (student == null)
                {
                    _logger.LogInformation("Student Not Found with ID = {0}!!", StudentID);
                    return null;
                }
                var studentDTO = _mapper.Map<StudentDTO>(student);
                return studentDTO;
            }
            catch (Exception)
            {
                throw;
            }

        }

        // UPDATE STUDENT
        public async Task<ResponseModel> UpdateStudentAsync(CreateStudentDTO StudentModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var student = await _studentService.GetByIdAsync(StudentModel.Id);
                if (student == null)
                {
                    model.IsSuccess = false;
                    model.Messsage = "Student Not Found with ID = {0}!!" + StudentModel.Id;
                    _logger.LogInformation("Student Not Found with ID = {0}!!", StudentModel.Id);
                    return model;
                }
                var std_upd = _mapper.Map<Student>(StudentModel);
                model = await _studentService.UpdateAsync(std_upd);
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error: " + ex.Message;
            }
            return model;
        }

        // DELETE STUDENT
        public async Task<ResponseModel> DeleteStudentAsync(Guid StudentID)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                model = await _studentService.DeleteAsync(StudentID);
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error: " + ex.Message;
            }
            return model;
        }

        public async Task<List<StudentDTO>> GetStudentDetailsByAgeG13Async(int age)
        {
            try
            {
                var std =await _studentService.Table.Where(s => s.Age == age).ToListAsync();
               // var stdd = std.Where(std => std == age);
                if (std == null) return null;
                
                var stdDTO = _mapper.Map<List<StudentDTO>>(std);

                return stdDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }

        
    }

}
