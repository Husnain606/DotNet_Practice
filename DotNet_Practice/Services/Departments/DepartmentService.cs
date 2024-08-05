using AutoMapper;
using DotNet_Practice.DTOs.RequestDTO;
using DotNet_Practice.DTOs.ResponseDTO;
using DotNet_Practice.Models;
using DotNet_Practice.Repository;
using DotNet_Practice.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DotNet_Practice.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepository<Department> _departmentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;
        private readonly ApplicationDbContext _dbContext;

        public DepartmentService(IRepository<Department> departmentRepository, IMapper mapper, ILogger<DepartmentService> logger, ApplicationDbContext dbContext)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _logger = logger;
            _dbContext = dbContext;
        }
        // CREATE DEPARTMENT
        public async Task<ResponseModel> CreateDepartmentAsync(CreateDepartmentDTO DepartmentModel)
        {
            ResponseModel model = new ResponseModel();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var department = _mapper.Map<Department>(DepartmentModel);
                department.Id = Guid.NewGuid();
                model = await _departmentRepository.CreateAsync(department);
                await transaction.CommitAsync();

                model.IsSuccess = true;
                model.Messsage = "Department Created Successfully";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while creating department.");
                throw ex;
            }
            return model;
        }

        // GET THE LIST OF ALL Departments
        public async Task<List<DepartmentDTO>> GetDepartmentListAsync()
        {
            try
            {
                _logger.LogInformation("Getting all the departments executed !!");
                var departments = await _departmentRepository.GetAllAsync();
                if (departments == null) return null;
                var departmentDTOs = _mapper.Map<List<DepartmentDTO>>(departments);
                return departmentDTOs;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting department list.");
                throw;
            }
        }

        // GET Department DETAILS BY Department ID
        public async Task<DepartmentDTO> GetDepartmentDetailsByIdAsync(Guid DepartmentID)
        {
            try
            {
                var department = await _departmentRepository.GetByIdAsync(DepartmentID);
                if (department == null)
                {
                    _logger.LogInformation("Department Not Found with ID = {0}!!", DepartmentID);
                    return null;
                }
                var departmentDTO = _mapper.Map<DepartmentDTO>(department);
                return departmentDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting department details.");
                throw;
            }
        }

        // UPDATE Department
        public async Task<ResponseModel> UpdateDepartmentAsync(CreateDepartmentDTO DepartmentModel)
        {
            ResponseModel model = new ResponseModel();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var department = await _departmentRepository.GetByIdAsync(DepartmentModel.Id);
                if (department == null)
                {
                    model.IsSuccess = false;
                    model.Messsage = $"Department Not Found with ID = {DepartmentModel.Id}!!";
                    _logger.LogInformation("Department Not Found with ID = {0}!!", DepartmentModel.Id);
                    return model;
                }
                var updatedDepartment = _mapper.Map<Department>(DepartmentModel);
                model = await _departmentRepository.UpdateAsync(updatedDepartment);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while updating department.");
                throw ex;
            }
            return model;
        }

        // DELETE Department
        public async Task<ResponseModel> DeleteDepartmentAsync(Guid DepartmentID)
        {
            ResponseModel model = new ResponseModel();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                model = await _departmentRepository.DeleteAsync(DepartmentID);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error occurred while deleting department.");
                model.IsSuccess = false;
                model.Messsage = "Error occurred while deleting department.";
            }
            return model;
        }
    }
}

