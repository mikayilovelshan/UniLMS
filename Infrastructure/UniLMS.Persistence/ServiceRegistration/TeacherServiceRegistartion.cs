using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.TeacherRepository;
using UniLMS.Persistence.Repositories.TeacherRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class TeacherServiceRegistartion
    {
        public static void AddTeacherPersistenceService(this IServiceCollection services)
        {
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<ITeacherReadRepository, TeacherReadRepository>();
            services.AddScoped<ITeacherWriteRepository, TeacherWriteRepository>();
        }
    }
}
