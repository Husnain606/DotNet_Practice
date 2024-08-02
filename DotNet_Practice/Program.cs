  using Microsoft.EntityFrameworkCore;
using DotNet_Practice.Services;
using DotNet_Practice.Configuration;
using Serilog;
using DotNet_Practice.Repository;
using FluentValidation.AspNetCore;
using DotNet_Practice.Models;
using DotNet_Practice.MiddleMiddleWare;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Log/Log.txt",rollingInterval : RollingInterval.Day )
    .CreateLogger();
        builder.Services.AddSerilog();


        // Add services to the container.  

        builder.Services.AddControllers().AddFluentValidation(
            v=> v.RegisterValidatorsFromAssemblyContaining<Student>())  ;
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Ading Db Context
        builder.Services.AddDbContext<SchoolContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStr")));

        //Registering MyService with its interface IMyService
        builder.Services.AddScoped(typeof(ISchoolRepository<>),typeof(SchoolRepository<>) );


        builder.Services.AddScoped(typeof(IStudentService), typeof(StudentService));

        //builder.Services.AddScoped<IStudentService, StudentService>();
        //Auto Mapper
        builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI();
        }
        app.UseMiddleware<ExceptionMiddleware>();
        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}