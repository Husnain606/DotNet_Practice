using AutoMapper;
using DotNet_Practice.Models;
using DotNet_Practice.DTOs.ResponseDTO;
using DotNet_Practice.DTOs.NewFolder;

namespace DotNet_Practice.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<CreateStudentDTO, Student>().ReverseMap();
            CreateMap<StudentDTO, Student>().ReverseMap().ForMember(d => d.Name, s => s.MapFrom(std => std.StudentFirstName + " " + std.StudentLastName))
             //   .ForMember(d => d.StudentLastName, s => s.Ignore())
                .ForMember(n => n.Class , op=> op.MapFrom(n => n.Class))  
                .AddTransform<String>(n => string.IsNullOrEmpty(n)?"Class not Found":n);
            // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
            CreateMap<DepartmentDTO, Department>().ReverseMap();

        }
    }
}
