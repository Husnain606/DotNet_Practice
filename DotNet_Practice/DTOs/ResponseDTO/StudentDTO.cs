using DotNet_Practice.Models;
namespace DotNet_Practice.DTOs.ResponseDTO
{
    public class StudentDTO
    {
        public string Name { get; set; } = string.Empty;
        public string StudentFatherName { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Mail { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;

    }
}
