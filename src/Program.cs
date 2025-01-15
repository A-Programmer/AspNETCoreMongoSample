using SchoolApi.Contracts;
using SchoolApi.ExtensionMethods;
using SchoolApi.Models;
using SchoolApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .Configure<SchoolDatabaseSettings>(
        builder.Configuration.GetSection("SchoolDatabaseSettings")
    );

builder.Services.AddControllers();

builder.AddMongoDbConnectionString();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IStudentServices, StudentServices>();
builder.Services.AddSingleton<ICourseServices, CourseServices>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
