using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocuSignHelper
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocuSignHelperServices(this IServiceCollection services)
        {
            services.AddScoped<IDocuSign, DocuSign>();
            services.AddMemoryCache();
            services.AddScoped<IMemoryCacheHelper, MemoryCacheHelper>();
            services.AddHostedService<MemoryCacheHelper>();

            return services;
        }
    }
}