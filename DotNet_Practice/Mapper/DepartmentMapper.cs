using AutoMapper;
using DotNet_Practice.Models;
using DotNet_Practice.DTOs.ResponseDTO;
using DotNet_Practice.DTOs.RequestDTO;

namespace DotNet_Practice.Configuration
{
    public class DepartmentMapper : Profile
    {
        public DepartmentMapper()
        {
            CreateMap<CreateDepartmentDTO, Department>().ReverseMap();
            CreateMap<DepartmentDTO, Department>().ReverseMap();
        }
    }
}
