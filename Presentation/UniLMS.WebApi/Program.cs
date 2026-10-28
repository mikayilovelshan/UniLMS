using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Reflection;
using System.Text;
using UniLMS.Application.ServiceRegistration;
using UniLMS.Domain.Entities;
using UniLMS.Persistence.Contexts;
using UniLMS.Persistence.ServiceRegistration;
using UniLMS.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddOpenApi();
builder.Services.AddIdentityServices();
builder.Services.AddFacultyPersistenceService();
builder.Services.AddCafedraPersistenceService();
builder.Services.AddCoursePersistenceService();
builder.Services.AddSpecialityPersistenceService();
builder.Services.AddStudentPersistenceService();
builder.Services.AddTeacherPersistenceService();
builder.Services.AddGroupPersistenceService();
builder.Services.AddSemesterPersistenceService();
builder.Services.AddCourseOfferingPersistenceService();
builder.Services.AddCourseSchedulePersistenceService();
builder.Services.AddAttendancePersistenceService();
builder.Services.AddExamResultPersistenceService();
builder.Services.AddFileServerPersistenceService();
builder.Services.AddApplicationService();

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
        ClockSkew = TimeSpan.Zero 
    };
});

var app = builder.Build();

app.UseGlobalExceptionMiddleware();

await app.SeedRolesAsync();

if (app.Environment.IsDevelopment())
{
    
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/openapi/{documentName}.json");
    });
}



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();