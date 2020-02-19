using System;
using ibon_poc.IRepositories;
using ibon_poc.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ibon_poc.Configurations
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<IHotSaleRepository, HotSaleRepository>();

            return services;
        }

    }
}
