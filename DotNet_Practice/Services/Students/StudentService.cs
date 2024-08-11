using DotNet_Practice.ViewModels;
using AutoMapper;
using DotNet_Practice.DTOs.ResponseDTO;
using DotNet_Practice.DTOs.NewFolder;
using Microsoft.EntityFrameworkCore;
using DotNet_Practice.Models;
using DotNet_Practice.Repository;
using DotNet_Practice.Common;
using DotNet_Practice.Services.Departments;
using DotNet_Practice.Utilities;

namespace DotNet_Practice.Services.Students
{
    public class StudentService : IStudentService
    {
        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Department> _departmentServices;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;
        private readonly ApplicationDbContext _dbContext;

        public StudentService(IRepository<Student> studentRepository, IMapper mapper, ILogger<StudentService> logger, ApplicationDbContext dbContext, IRepository<Department> departmentServices)
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
                int i = 0;
                foreach (var studentDTO in studentDTOs)  
                {
                    studentDTO.timespann = CheckTime.GetTimeDifference(students[i].EnrollmentDate);
                    i++;
                }
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
                studentDTO.timespann= CheckTime.GetTimeDifference(student.EnrollmentDate);
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
        public async Task<List<StudentDTO>> InnerJoin()
        {
            try
            {
                var departments = await _departmentServices.GetAllAsync();
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
        //Get Left Outer Join Fields
        public async Task<List<StudentDTO>> GetLeftOuterJoinFields()
        {
            try
            {
                // Fetch the list of departments from the database
                var departmentDTO = await _departmentServices.GetAllAsync();
                var departments = _mapper.Map<List<Department>>(departmentDTO);

                // Fetch the list of students from the database
                var students = await _studentRepository.Table.AsNoTracking().ToListAsync();

                // Perform the left outer join in-memory
                var leftOuterJoin = from student in students
                                    join department in departments
                                    on student.DepartmentId equals department.Id into deptGroup
                                    from dept in deptGroup.DefaultIfEmpty() // This ensures that all students are included
                                    select new
                                    {
                                        student.StudentFirstName,
                                        student.Age,
                                        DepartmentDescription = dept?.DepartmenrDescription
                                    };
                // Map the results to StudentDTO
                var result = leftOuterJoin.Select(x => new StudentDTO
                {
                    Name = x.StudentFirstName,
                    Age = x.Age,
                    Department = x.DepartmentDescription ?? "No Department"
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting left outer join results.");
                throw;
            }
        }
        // Get Right Outer Join Fields
        public async Task<List<StudentDTO>> GetRightOuterJoinFields()
        {
            try
            {
                // Fetch the list of departments from the database
                var departmentDTO = await _departmentServices.GetAllAsync();
                var departments = _mapper.Map<List<Department>>(departmentDTO);

                // Fetch the list of students from the database
                var students = await _studentRepository.Table.AsNoTracking().ToListAsync();

                // Perform the right outer join in-memory (by swapping the tables)
                var rightOuterJoin = from department in departments
                                     join student in students
                                     on department.Id equals student.DepartmentId into studentGroup
                                     from stud in studentGroup.DefaultIfEmpty() // This ensures that all departments are included
                                     select new
                                     {
                                         StudentFirstName = stud?.StudentFirstName,
                                         Age = stud?.Age,
                                         DepartmentDescription = department.DepartmenrDescription
                                     };

                // Map the results to StudentDTO
                var result = rightOuterJoin.Select(x => new StudentDTO
                {
                    Name = x.StudentFirstName ?? "No Student",
                    Age = x.Age ?? 0,
                    Department = x.DepartmentDescription
                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting right outer join results.");
                throw;
            }
        }
        //Get Left Inner Join Fields
        public async Task<List<StudentDTO>> GetLeftInnerJoinFields()
        {
            try
            {
                var departmentDTO = await _departmentServices.GetAllAsync();
                var departments = _mapper.Map<List<Department>>(departmentDTO);

                var students = await _studentRepository.Table.AsNoTracking().ToListAsync();

                var innerJoin = students
                    .Join(departments,
                        student => student.DepartmentId,
                        department => department.Id,
                        (student, department) => new
                        {
                            student.StudentFirstName,
                            student.Age,
                            department.DepartmenrDescription
                        })
                    .ToList();

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
                _logger.LogError(ex, "Error occurred while getting left inner join results.");
                throw;
            }
        }
        //Get Right Inner Join Fields
        public async Task<List<StudentDTO>> GetRightInnerJoinFields()
        {
            try
            {
                var departmentDTO = await _departmentServices.GetAllAsync();
                var departments = _mapper.Map<List<Department>>(departmentDTO);

                var students = await _studentRepository.Table.AsNoTracking().ToListAsync();

                var innerJoin = departments
                    .Join(students,
                        department => department.Id,
                        student => student.DepartmentId,
                        (department, student) => new
                        {
                            student.StudentFirstName,
                            student.Age,
                            department.DepartmenrDescription
                        })
                    .ToList();
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
                _logger.LogError(ex, "Error occurred while getting right inner join results.");
                throw;
            }
        }
     //   Get Group Join Fields
        public async Task<List<StudentDTO>> GetGroupJoinFields()
        {
            try
            {
                // Retrieve all departments
                var departmentDTO = await _departmentServices.GetAllAsync();
                var departments = _mapper.Map<List<Department>>(departmentDTO);

                // Retrieve all students
                var students = await _studentRepository.Table.AsNoTracking().ToListAsync();

                // Perform Group Join
                var groupJoin = departments
                    .GroupJoin(
                        students,
                        department => department.Id, // Key selector for departments
                        student => student.DepartmentId, // Key selector for students
                        (department, studentGroup) => new
                        {
                            DepartmentName = department.DepartmenrDescription,
                            Students = studentGroup
                        })
                    .ToList();

                // Flatten the results and map to StudentDTO
                var result = groupJoin
                    .SelectMany(
                        dept => dept.Students.DefaultIfEmpty(), // Handle the case where there might be no students in a department
                        (dept, student) => new StudentDTO
                        {
                            Name = student?.StudentFirstName ?? "No Student", // Handle null case for students
                            Age = student?.Age ?? 0, // Handle null case for age
                            Department = dept.DepartmentName
                        })
                    .ToList();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting group join results.");
                throw;
            }
        }
        // GROUP BY DEPARTMENT
        public async Task<List<GroupedStudentsDTO>> GroupByDepartment()
        {
            try
            {
                var students = await _studentRepository.Table.AsNoTracking().ToListAsync();

                var groupedStudents = students
                    .GroupBy(student => student.DepartmentId)
                    .Select(group => new GroupedStudentsDTO
                    {
                        DepartmentId = group.Key,
                        Students = group.Select(student => new StudentDTO
                        {
                            Name = student.StudentFirstName  + student.StudentLastName,
                            Mail= student.Mail,
                            Class=  student.Class,
                            Contact= student.Contact,
                            Age = student.Age
                        }).ToList()
                    })
                    .ToList();

                return groupedStudents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while grouping students by department.");
                throw;
            }
        }

    }
}

