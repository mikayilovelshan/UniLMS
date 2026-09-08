using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.GroupRepository;
using UniLMS.Persistence.Repositories.GroupRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class GroupServiceRegistration
    {
        public static void AddGroupPersistenceService(this IServiceCollection services)
        {
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IGroupReadRepository, GroupReadRepository>();
            services.AddScoped<IGroupWriteRepository, GroupWriteRepository>();
        }
    }
}
