using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.FacultyRepository;
using UniLMS.Persistence.Repositories.FacultyRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class FacultyServiceRegistration
    {
        public static void AddFacultyPersistenceService( this IServiceCollection services)
        {
            services.AddScoped<IFacultyReadRepository, FacultyReadRepository>();
            services.AddScoped<IFacultyWriteRepository, FacultyWriteRepository>();
            services.AddScoped<IFacultyService, FacultyService>();  
        }
    }
}
