using DotNet_Practice.Models;
using DotNet_Practice.Repository;

namespace DotNet_Practice.Controllers.StdController
{
    public interface IStudentRepository :ISchoolRepository<Student>
    {

        Task<List<Student>> GetByFeesStatus();  
    }
}
