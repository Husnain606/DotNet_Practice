using Microsoft.EntityFrameworkCore;
using DotNet_Practice.Configuration;
using Serilog;
using DotNet_Practice.Repository;
using DotNet_Practice.MiddleMiddleWare;
using DotNet_Practice.Services.Departments;
using DotNet_Practice.Services.Students;
using FluentValidation;
using DotNet_Practice.DTOs.NewFolder;
using DotNet_Practice.Validators;
using DotNet_Practice.DTOs.RequestDTO;


internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration().WriteTo.File("Log/Log.txt", rollingInterval: RollingInterval.Day).CreateLogger();
        builder.Services.AddSerilog();

        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        builder.Services.AddTransient<IValidator<CreateStudentDTO>, CreateStudentValidator>();
        builder.Services.AddTransient<IValidator<CreateDepartmentDTO>, DepartmentValidator>();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        //Ading Db Context
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStr")));

        //Registering MyService with its interface IMyService
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped(typeof(IStudentService), typeof(StudentService));
        builder.Services.AddScoped(typeof(IDepartmentService), typeof(DepartmentService));

        //Auto Mapper
        builder.Services.AddAutoMapper(typeof(StudentMapper));
        builder.Services.AddAutoMapper(typeof(DepartmentMapper));
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