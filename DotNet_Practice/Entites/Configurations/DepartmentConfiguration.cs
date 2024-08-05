﻿using DotNet_Practice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNet_Practice.Entites.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(s => s.Id).ValueGeneratedNever();
            builder.Property(d => d.DepartmentName)
                .IsRequired()
                .HasMaxLength(50);
            builder.Property(d => d.DepartmenrDescription)
                .HasMaxLength(200);
            // Configuring the relationship with Student entity
            builder.HasMany(d => d.Student)
                .WithOne(s => s.Department)
                .HasForeignKey(s => s.DepartmentId);
        }
    }
}
