using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.CourseOfferingRepository;
using UniLMS.Persistence.Repositories.CourseOfferingRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class CourseOfferingServiceRegistration
    {
        public static void AddCourseOfferingPersistenceService(this IServiceCollection services)
        {
            services.AddScoped<ICourseOfferingService, CourseOfferingService>();
            services.AddScoped<ICourseOfferingReadRepository, CourseOfferingReadRepository>();
            services.AddScoped<ICourseOfferingWriteRepository, CourseOfferingWriteRepository>();
            
        }
    }
}
