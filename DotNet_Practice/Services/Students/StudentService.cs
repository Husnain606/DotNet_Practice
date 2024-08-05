using DotNet_Practice.ViewModels;
using AutoMapper;
using DotNet_Practice.DTOs.ResponseDTO;
using DotNet_Practice.DTOs.NewFolder;
using Microsoft.EntityFrameworkCore;
using DotNet_Practice.Models;
using DotNet_Practice.Repository;
using DotNet_Practice.Common;
using DotNet_Practice.Services.Departments;

namespace DotNet_Practice.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Department> _departmentServices;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;
        private readonly ApplicationDbContext _dbContext;

        public StudentService(IRepository<Student> studentRepository, IMapper mapper, ILogger<StudentService> logger, ApplicationDbContext dbContext , IRepository<Department> departmentServices)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
            _departmentServices = departmentServices;
           
        }

        // CREATE STUDENT
        public async Task<ResponseModel> CreateStudentAsync(CreateStudentDTO studentModel)
        {
            ResponseModel model = new ResponseModel();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var student = _mapper.Map<Student>(studentModel);
                student.Id = Guid.NewGuid();
                model = await _studentRepository.CreateAsync(student);
                await transaction.CommitAsync();

                model.IsSuccess = true;
                model.Messsage = StudentConstants.successMessage;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while creating student.");
                throw ex;
            }
            return model;
        }

        // GET THE LIST OF ALL STUDENTS
        public async Task<List<StudentDTO>> GetStudentListAsync()
        {
            try
            {
                _logger.LogInformation("Getting all the students executed !!");
                var students = await _studentRepository.GetAllAsync();
                if (students == null) return null;
                var studentDTOs = _mapper.Map<List<StudentDTO>>(students);
                return studentDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting student list.");
                throw;
            }
        }

        // GET STUDENT DETAILS BY STUDENT ID
        public async Task<StudentDTO> GetStudentDetailsByIdAsync(Guid studentId)
        {
            try
            {
                var student = await _studentRepository.GetByIdAsync(studentId);
                if (student == null)
                {
                    _logger.LogInformation(StudentConstants.notFound);
                    return null;
                }
                var studentDTO = _mapper.Map<StudentDTO>(student);
                return studentDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting student details.");
                throw;
            }
        }

        // UPDATE STUDENT
        public async Task<ResponseModel> UpdateStudentAsync(CreateStudentDTO studentModel)
        {
            ResponseModel model = new ResponseModel();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var student = await _studentRepository.GetByIdAsync(studentModel.Id);
                if (student == null)
                {
                    model.IsSuccess = false;
                    model.Messsage = StudentConstants.notFound;
                    _logger.LogInformation(StudentConstants.notFound, studentModel.Id);
                    return model;
                }
                var updatedStudent = _mapper.Map<Student>(studentModel);
                model = await _studentRepository.UpdateAsync(updatedStudent);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while updating student.");
                throw ex;
            }
            return model;
        }

        // DELETE STUDENT
        public async Task<ResponseModel> DeleteStudentAsync(Guid studentId)
        {
            ResponseModel model = new ResponseModel();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                model = await _studentRepository.DeleteAsync(studentId);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while deleting student.");
                throw;
            }
            return model;
        }
        // GET DETAIL BY SPECIFICATION
        public async Task<List<StudentDTO>> GetStudentDetailsByAgeG13Async(int age)
        {
            try
            {
                var students = await _studentRepository.Table.Where(s => s.Age == age).Distinct().ToListAsync();
                if (students == null) return null;

                var studentDTOs = _mapper.Map<List<StudentDTO>>(students);
                return studentDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting students by age.");
                throw;
            }
        }


        // GET DETAIL BY SPECIFICATION
        public async Task<List<StudentDTO>> GetSpecificFields()
        {
            try
            {
                var departments= await _departmentServices.GetAllAsync();
                // Fetch the list of students from the database
                var students = await _studentRepository.Table.AsNoTracking().ToListAsync();

                // Perform the join in-memory
                var innerJoin = from student in students
                                join department in departments
                                on student.DepartmentId equals department.Id
                                select new
                                {
                                    student.StudentFirstName,
                                    student.Age,
                                    department.DepartmenrDescription
                                };

                // Map the results to StudentDTO
                var result = innerJoin.Select(x => new StudentDTO
                {
                    Name = x.StudentFirstName,
                    Age = x.Age,
                    Department = x.DepartmenrDescription
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting students by age.");
                throw;
            }
        }
    }
}

