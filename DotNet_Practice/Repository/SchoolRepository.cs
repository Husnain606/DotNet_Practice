using DotNet_Practice.Models;
using DotNet_Practice.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DotNet_Practice.Repository
{
   
    public class SchoolRepository<T> : ISchoolRepository<T> where T:class
    {
        private readonly SchoolContext _dbContext;
       // public IQueryable<T> TableNoTracking => Entities.AsNoTracking();
        public SchoolRepository(SchoolContext dbContext) 
        {
            _dbContext = dbContext;
        
        }


        // CREATE 
        public async Task<ResponseModel> CreateAsync(T CreateModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                await _dbContext.AddAsync<T>(CreateModel);
                await _dbContext.SaveChangesAsync();
                model.IsSuccess = true;
                model.Messsage = "Student Created Successfully";
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error: " + ex.Message;
            }
            return model;
        }

        // GET THE LIST OF ALL 
        public async Task<List<T>> GetAllAsync()
        {
            List<T> GETList;
            try
            {
                GETList = await _dbContext.Set<T>().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return GETList;
        }

        // GET  DETAILS BY  ID
        public async Task<T> GetByIdAsync(int ID)
        {
            T GetById;
            try
            {
                GetById = await _dbContext.FindAsync<T>(ID);
            }
            catch (Exception)
            {
                throw;
            }
            return GetById;
        }

        // UPDATE 
        public async Task<ResponseModel> UpdateAsync(T UpdateModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                _dbContext.Update<T>(UpdateModel);
                model.Messsage = "Student Updated Successfully";
               
                await _dbContext.SaveChangesAsync();
                model.IsSuccess = true;
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error: " + ex.Message;
            }
            return model;
        }


        // DELETE 
        public async Task<ResponseModel> DeleteAsync(int ID)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                T _temp = await GetByIdAsync(ID);
                if (_temp != null)
                {
                    _dbContext.Remove<T>(_temp);
                    await _dbContext.SaveChangesAsync();
                    model.IsSuccess = true;
                    model.Messsage = "Student Deleted Successfully";
                }
                else
                {
                    model.IsSuccess = false;
                    model.Messsage = "Student Not Found";
                }
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error: " + ex.Message;
            }
            return model;
        }

    }
}

        
    

