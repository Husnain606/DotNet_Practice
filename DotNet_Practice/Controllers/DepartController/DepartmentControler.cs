using AutoMapper;
using DotNet_Practice.Models;
using DotNet_Practice.Repository;
using Microsoft.AspNetCore.Mvc;
using DotNet_Practice.DTOs.ResponseDTO;

namespace DotNet_Practice.Controllers.DepartController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly ISchoolRepository<Department> _departmentService;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentController> _logger;

        public DepartmentController(ISchoolRepository<Department> service, IMapper mapper, ILogger<DepartmentController> logger)
        {
            _departmentService = service;
            _mapper = mapper;
            _logger = logger;
        }

        // GET ALL DEPARTMENTS
        [HttpGet("GetAllDepartments")]
        public async Task<IActionResult> GetAllDepartments()
        {
            try
            {
                _logger.LogInformation("Getting all the departments executed !!");
                var departments = await _departmentService.GetAllAsync();
                if (departments == null) return NotFound();
                var departmentDTOs = _mapper.Map<List<DepartmentDTO>>(departments);
                return Ok(departmentDTOs);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // GET DEPARTMENT DETAILS BY ID
        [HttpGet("GetDepartmentById/{id}")]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            try
            {
                var department = await _departmentService.GetByIdAsync(id);
                if (department == null)
                {
                    _logger.LogInformation("Department Not Found with ID = {0}!!", id);
                    return NotFound();
                }
                var departmentDTO = _mapper.Map<DepartmentDTO>(department);
                return Ok(departmentDTO);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // CREATE DEPARTMENT
        [HttpPost("CreateDepartment")]
        public async Task<IActionResult> CreateDepartment(Department departmentModel)
        {
            try
            {
                var model = await _departmentService.CreateAsync(departmentModel);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // UPDATE DEPARTMENT
        [HttpPut("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment(Department departmentModel)
        {
            try
            {
                var model = await _departmentService.UpdateAsync(departmentModel);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        // DELETE DEPARTMENT
        [HttpDelete("DeleteDepartment/{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var model = await _departmentService.DeleteAsync(id);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }

}
