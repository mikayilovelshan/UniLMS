using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace UniLMS.Application.ServiceRegistration
{
    public static class ApplicationServiceRegistration
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { } ,Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
