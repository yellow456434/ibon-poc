using System;
using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Order.QueueConsumer;
using Order.Services;

namespace Order
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private readonly IBusControl _busControl;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host("rabbitmq://" + Environment.GetEnvironmentVariable("rabbitmqHost") + ":" + Configuration["rabbitmqPort"],
                        host => {
                            host.Username("admin");
                            host.Password("pw12345");
                        });

                    cfg.ReceiveEndpoint("queueTest", e =>
                    {
                        e.PrefetchCount = 1;
                        e.UseMessageRetry(x => x.Interval(2, 100));

                        e.ConfigureConsumer<OrderConsumer>(provider);
                    });
                }));

                x.AddConsumer<OrderConsumer>();
            });

            services.AddSingleton<IHostedService, BusService>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
