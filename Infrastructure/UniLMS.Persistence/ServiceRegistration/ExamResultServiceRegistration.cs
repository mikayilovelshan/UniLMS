using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using UniLMS.Application.Interfaces.Services;
using UniLMS.Application.Repositories.ExamResultRepository;
using UniLMS.Persistence.Repositories.ExamResultRepository;
using UniLMS.Persistence.Services;

namespace UniLMS.Persistence.ServiceRegistration
{
    public static class ExamResultServiceRegistration
    {
        public static void AddExamResultPersistenceService(this IServiceCollection services)
        {
            services.AddScoped<IExamResultService, ExamResultService>();
            services.AddScoped<IExamResultReadRepository, ExamResultReadRepository>();
            services.AddScoped<IExamResultWriteRepository, ExamResultWriteRepository>();
            
        }
    }
}
