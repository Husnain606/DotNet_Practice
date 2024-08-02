﻿using DotNet_Practice.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace DotNet_Practice.Repository
{
    public class SchoolRepository<T> : ISchoolRepository<T> where T : class
    {
        private readonly SchoolContext _dbContext;
        private DbSet<T> _entities;
        public SchoolRepository(SchoolContext dbContext)
        {
            _dbContext = dbContext;
        }

        // IQueryable<T> implementation
        protected virtual DbSet<T> Entities => _entities ??= _dbContext.Set<T>();

        public DbSet<T> Table => Entities;

        // IQueryable<T> with AsNoTracking
        public IQueryable<T> TableNoTracking => _dbContext.Set<T>().AsNoTracking();


        // CREATE 
        public async Task<ResponseModel> CreateAsync(T createModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                await _dbContext.AddAsync<T>(createModel);
                await _dbContext.SaveChangesAsync();
                model.IsSuccess = true;
                model.Messsage = "Entity Created Successfully";
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
            try
            {
                return await _dbContext.Set<T>().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET THE LIST OF ALL with AsNoTracking
        public async Task<List<T>> GetAllNoTrackingAsync()
        {
            try
            {
                return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET  DETAILS BY  ID
        public async Task<T> GetByIdAsync(Guid id)
        {
            try
            {
                return await _dbContext.FindAsync<T>(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // UPDATE 
        public async Task<ResponseModel> UpdateAsync(T updateModel)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                _dbContext.Update<T>(updateModel);
                await _dbContext.SaveChangesAsync();
                model.IsSuccess = true;
                model.Messsage = "Entity Updated Successfully";
            }
            catch (Exception ex)
            {
                model.IsSuccess = false;
                model.Messsage = "Error: " + ex.Message;
            }
            return model;
        }

        // DELETE 
        public async Task<ResponseModel> DeleteAsync(Guid id)
        {
            ResponseModel model = new ResponseModel();
            try
            {
                T entity = await GetByIdAsync(id);
                if (entity != null)
                {
                    _dbContext.Remove<T>(entity);
                    await _dbContext.SaveChangesAsync();
                    model.IsSuccess = true;
                    model.Messsage = "Entity Deleted Successfully";
                }
                else
                {
                    model.IsSuccess = false;
                    model.Messsage = "Entity Not Found";
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
