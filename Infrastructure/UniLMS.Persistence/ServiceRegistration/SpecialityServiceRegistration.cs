using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.SpecialityRepository;
using UniLMS.Persistence.Repositories.SpecialityRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class SpecialityServiceRegistration 
    {
        public static void AddSpecialityPersistenceService(this IServiceCollection services)
        {
            services.AddScoped<ISpecialityReadRepository, SpecialityReadRepository>();
            services.AddScoped<ISpecialityWriteRepository, SpecialityWriteRepository>();
            services.AddScoped<ISpecialityService, SpecialityService>();
        }
    }
}
