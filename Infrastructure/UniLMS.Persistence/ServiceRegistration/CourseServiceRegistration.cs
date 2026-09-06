using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.CourseRepository;
using UniLMS.Persistence.Repositories.CourseRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class CourseServiceRegistration
    {
        public static void AddCoursePersistenceService(this IServiceCollection services)
        {
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICourseReadRepository, CourseReadRepository>();
            services.AddScoped<ICourseWriteRepository, CourseWriteRepository>();

        }
    }
}
