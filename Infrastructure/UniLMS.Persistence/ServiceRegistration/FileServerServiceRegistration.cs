using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Infrastructure;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class FileServerServiceRegistration
    {
        public static void AddFileServerPersistenceService(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
        }
    }
}
