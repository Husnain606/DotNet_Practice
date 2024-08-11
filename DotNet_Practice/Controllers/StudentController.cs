using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DotNet_Practice.DTOs.NewFolder;
using DotNet_Practice.Services.Students;

namespace DotNet_Practice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {

        private readonly IStudentService studentServices;
        private readonly IMapper _mapper;
        private readonly ILogger<StudentController> _logger;

        public StudentController(IMapper mapper, ILogger<StudentController> logger, IStudentService _studentServices)
        {
            _mapper = mapper;
            _logger = logger;
            studentServices = _studentServices;
        }

        // GET ALL STUDENT
        [HttpGet("GetAllStudents")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await studentServices.GetStudentListAsync();
                if (students == null) return NotFound();
                return Ok(students);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET STUDENT DETAILS BY ID
        [HttpGet("GetStudentById/{id}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            try
            {
                var student = await studentServices.GetStudentDetailsByIdAsync(id);
                if (student == null)
                {
                    _logger.LogInformation("Student Not Found with ID = {0}!!", id);
                    return NotFound();
                }

                return Ok(student);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET STUDENT DETAILS BY  JOINS 
        [HttpGet("GetStudentByAge\"")]
        public async Task<IActionResult> GetStudentByAge()
        {
            try
            {
                var student = await studentServices.GroupByDepartment();
                if (student == null)
                {
                    _logger.LogInformation("Student Not Found with Age = {0}!!");
                    return NotFound();
                }
                return Ok(student);
            }
            catch (Exception)
            {
                throw;
            }
        }


        // CREATE STUDENT
        [HttpPost("CreateStudent")]
        public async Task<IActionResult> CreateStudent(CreateStudentDTO studentModel)
        {
            try
            {
                var model = await studentServices.CreateStudentAsync(studentModel);
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // UPDATE STUDENT
        [HttpPut("UpdateStudent")]
        public async Task<IActionResult> UpdateStudent(CreateStudentDTO studentModel)
        {
            try
            {
                var model = await studentServices.UpdateStudentAsync(studentModel);
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // DELETE STUDENT
        [HttpDelete("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            try
            {
                var model = await studentServices.DeleteStudentAsync(id);
                return Ok(model);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}
