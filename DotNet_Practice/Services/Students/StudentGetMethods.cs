using AutoMapper;
using DotNet_Practice.DTOs.ResponseDTO;
using Microsoft.EntityFrameworkCore;
using DotNet_Practice.Models;
using DotNet_Practice.Repository;
namespace DotNet_Practice.Services.Students
{
    public class StudentGetMethods : IStudentGetMethods
    {

        private readonly IRepository<Student> _studentRepository;
        private readonly IRepository<Department> _departmentServices;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentService> _logger;
        private readonly ApplicationDbContext _dbContext;

        public StudentGetMethods(IRepository<Student> studentRepository, IMapper mapper, ILogger<StudentService> logger, ApplicationDbContext dbContext, IRepository<Department> departmentServices)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
            _departmentServices = departmentServices;

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
                            Name = student.StudentFirstName + student.StudentLastName,
                            Mail = student.Mail,
                            Class = student.Class,
                            Contact = student.Contact,
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
