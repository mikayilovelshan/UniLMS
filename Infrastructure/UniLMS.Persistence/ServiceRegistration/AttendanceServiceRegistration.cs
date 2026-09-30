using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.AttendanceRepository;
using UniLMS.Persistence.Repositories.AttendanceRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class AttendanceServiceRegistration
    {
        public static void AddAttendancePersistenceService(this IServiceCollection services)
        {
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IAttendanceReadRepository, AttendanceReadRepository>();
            services.AddScoped<IAttendanceWriteRepository, AttendanceWriteRepository>();
        }
    }
}
