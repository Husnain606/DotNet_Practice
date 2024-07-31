using DotNet_Practice.Models;
using DotNet_Practice.Repository;
using Microsoft.EntityFrameworkCore;

namespace DotNet_Practice.Controllers.StdController
{
    public class StudentRepository : SchoolRepository<Student>, IStudentRepository
    {
        private readonly SchoolContext _dbContext;

        public StudentRepository(SchoolContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<List<Student>> GetByFeesStatus()
        {
            List<Student> stdList;
            try
            {
                stdList = await _dbContext.Set<Student>().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return stdList;
        }

    }
}
