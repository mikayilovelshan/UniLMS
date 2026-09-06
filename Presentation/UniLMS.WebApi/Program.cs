using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Reflection;
using UniLMS.Application.ServiceRegistration;
using UniLMS.Persistence.Contexts;
using UniLMS.Persistence.ServiceRegistration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddOpenApi();
builder.Services.AddFacultyPersistenceService();
builder.Services.AddCafedraPersistenceService();
builder.Services.AddCoursePersistenceService();
builder.Services.AddSpecialityPersistenceService();
builder.Services.AddApplicationService();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/openapi/{documentName}.json");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();