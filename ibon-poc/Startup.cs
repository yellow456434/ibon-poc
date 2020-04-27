using System;
using GreenPipes;
using ibon_poc.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ibon_poc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContextPool<BookDbContext>(opts => opts.UseSqlServer(Configuration.GetConnectionString("Book")).EnableSensitiveDataLogging());

            //services.ConfigureRepositories();

            services.AddMassTransit(x =>
            {               
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host("rabbitmq://" + Environment.GetEnvironmentVariable("rabbitmqHost") + ":" + Configuration["rabbitmqPort"],
                        host =>
                        {
                            host.Username(Configuration["rabbitmq:username"]);
                            host.Password(Configuration["rabbitmq:password"]);
                        });
                }));
            });

            //services.AddSingleton<IHostedService, BusService>();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
