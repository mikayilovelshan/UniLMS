using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.CourseScheduleRepository;
using UniLMS.Persistence.Repositories;
using UniLMS.Persistence.Repositories.CourseScheduleRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class CourseScheduleServiceRegistration
    {
        public static void AddCourseSchedulePersistenceService(this IServiceCollection services)
        {
            services.AddScoped<ICourseScheduleService, CourseScheduleService>();
            services.AddScoped<ICourseScheduleReadRepository, CourseScheduleReadRepository>();
            services.AddScoped<ICourseScheduleWriteRepository, CourseScheduleWriteRepository>();
        }
    }
}
