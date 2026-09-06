using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.CafedraRepository;
using UniLMS.Persistence.Repositories.CafedraRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class CafedraServiceRegistration
    {
        public static void AddCafedraPersistenceService(this IServiceCollection services)
        {
            services.AddScoped<ICafedraService, CafedraService>();
            services.AddScoped<ICafedraWriteRepository, CafedraWriteRepository>();
            services.AddScoped<ICafedraReadRepository, CafedraReadRepository>();
            
        }
    }
}
