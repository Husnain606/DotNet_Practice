using AutoMapper;
using DotNet_Practice.Models;
using DotNet_Practice.DTOs.ResponseDTO;
using DotNet_Practice.DTOs.NewFolder;

namespace DotNet_Practice.Configuration
{
    public class StudentMapper : Profile
    {
        public StudentMapper()
        {
            CreateMap<CreateStudentDTO, Student>().ReverseMap();
            CreateMap<StudentDTO, Student>().ReverseMap().ForMember(d => d.Name, s => s.MapFrom(std => std.StudentFirstName + " " + std.StudentLastName))
                .ForMember(d => d.timespann, s => s.MapFrom(std => (DateTime.Now - std.EnrollmentDate) ))
                .ForMember(n => n.Class, op => op.MapFrom(n => n.Class))
                .AddTransform<String>(n => string.IsNullOrEmpty(n) ? "Class not Found" : n);
            //   .ForMember(d => d.StudentLastName, s => s.Ignore())
            // Use CreateMap... Etc.. here (Profile methods are the same as configuration methods)
        }
    }
}
