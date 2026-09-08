using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.StudentRepository;
using UniLMS.Persistence.Repositories.StudentRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class StudentServiceRegistration
    {
        public static void AddStudentPersistenceService(this IServiceCollection services)
        {
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IStudentReadRepository, StudentReadRepository>();
            services.AddScoped<IStudentWriteRepository, StudentWriteRepository>();

        }
    }
}
