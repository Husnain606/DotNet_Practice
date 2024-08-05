
using DotNet_Practice.Entites.Configurations;
using DotNet_Practice.Models;
using Microsoft.EntityFrameworkCore;


namespace DotNet_Practice.Repository
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Department> Department { get; set; }
        public DbSet<Student> Student { get; set; }
        public ApplicationDbContext(DbContextOptions options)
        : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration<Student>(new StudentConfiguration());
            modelBuilder.ApplyConfiguration<Department>(new DepartmentConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
