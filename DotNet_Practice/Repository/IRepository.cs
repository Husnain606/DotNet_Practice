using DotNet_Practice.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DotNet_Practice.Repository
{
    public interface IRepository<T> where T : class 
    {
        Task<ResponseModel> CreateAsync(T CreateModel);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid ID);
        Task<ResponseModel> UpdateAsync(T UpdateModel);    
        Task<ResponseModel> DeleteAsync(Guid ID);
        DbSet<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
    }
}
