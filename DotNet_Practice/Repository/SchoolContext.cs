﻿using DotNet_Practice.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNet_Practice.Repository
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions options)
        : base(options)
        {
        }
        public DbSet<Department> Department { get; set; }
        public DbSet<Student> Student { get; set; }
  


    }
}
