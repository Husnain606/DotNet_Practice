using DotNet_Practice.ViewModels;

namespace DotNet_Practice.Repository
{
    public interface ISchoolRepository<T> where T : class 
    {
        Task<ResponseModel> CreateAsync(T CreateModel);
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int ID);
        Task<ResponseModel> UpdateAsync(T UpdateModel);    
        Task<ResponseModel> DeleteAsync(int ID);
    }
}
