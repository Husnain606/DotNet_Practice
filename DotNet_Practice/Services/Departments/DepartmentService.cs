using AutoMapper;
using DotNet_Practice.DTOs.RequestDTO;
using DotNet_Practice.DTOs.ResponseDTO;
using DotNet_Practice.Models;
using DotNet_Practice.Repository;
using DotNet_Practice.ViewModels;

namespace DotNet_Practice.Services.Departments
{
    public class DepartmentService : IDepartmentService
    {

        private readonly IRepository<Department> _departmentService;
        private readonly IMapper _mapper;
        private readonly ILogger<DepartmentService> _logger;



        public DepartmentService(IRepository<Department> service, IMapper mapper, ILogger<DepartmentService> logger)
        {
            _departmentService = service;
            _mapper = mapper;
            _logger = logger;

        }
        public async Task<ResponseModel> CreateDepartmentAsync(CreateDepartmentDTO DepartmentModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var department = _mapper.Map<Department>(DepartmentModel);
                department.Id= Guid.NewGuid();
                model = await _departmentService.CreateAsync(department);
                model.IsSuccess = true;
                model.Messsage = "Department Created Successfully";
            }
            catch (Exception x)
            {
                throw;
            }
            return model;
        }

        // GET THE LIST OF ALL Department
        public async Task<List<DepartmentDTO>> GetDepartmentListAsync()
        {
            try
            {
                _logger.LogInformation("Getting all the departments executed !!");
                var departments = await _departmentService.GetAllAsync();
                if (departments == null) return null;
                var departmentDTOs = _mapper.Map<List<DepartmentDTO>>(departments);
                return departmentDTOs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET Department DETAILS BY Department ID
        public async Task<DepartmentDTO> GetDepartmentDetailsByIdAsync(Guid DepartmentID)
        {
            Department std;
            try
            {
                var department = await _departmentService.GetByIdAsync(DepartmentID);
                if (department == null)
                {
                    _logger.LogInformation("Department Not Found with ID = {0}!!", DepartmentID);
                    return null;
                }
                var departmentDTO = _mapper.Map<DepartmentDTO>(department);
                return departmentDTO;
            }
            catch (Exception)
            {
                throw;
            }

        }

        // UPDATE Department
        public async Task<ResponseModel> UpdateDepartmentAsync(CreateDepartmentDTO DepartmentModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                var department = await _departmentService.GetByIdAsync(DepartmentModel.Id);
                if (department == null)
                {
                    model.IsSuccess = false;
                    model.Messsage = "Department Not Found with ID = {0}!!" + DepartmentModel.Id;
                    _logger.LogInformation("Department Not Found with ID = {0}!!", DepartmentModel.Id);
                    return model;
                }
                var std_upd = _mapper.Map<Department>(DepartmentModel);
                model = await _departmentService.UpdateAsync(std_upd);
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }

        // DELETE Department
        public async Task<ResponseModel> DeleteDepartmentAsync(Guid DepartmentID)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                model = await _departmentService.DeleteAsync(DepartmentID);
            }
            catch (Exception)
            {
                throw;
            }
            return model;
        }
    }
}
