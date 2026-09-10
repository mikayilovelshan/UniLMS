using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.SemesterRepository;
using UniLMS.Persistence.Repositories.SemesterRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class SemesterServiceRegistartion
    {
        public static void AddSemesterPersistenceService(this IServiceCollection services)
        {
            services.AddScoped<ISemesterService, SemesterService>();
            services.AddScoped<ISemesterReadRepository, SemesterReadRepository>();
            services.AddScoped<ISemesterWriteRepository, SemesterWriteRepository> ();
        }
    }
}
