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
                            host.Username(Configuration["rabbitmq:username"]);
                            host.Password(Configuration["rabbitmq:password"]);
                        });

                    cfg.ReceiveEndpoint("queueTest", e =>
                    {
                        e.PrefetchCount = 1;
                        e.UseMessageRetry(x => x.Intervals(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(15)));

                        e.ConfigureConsumer<OrderConsumer>(provider);
                        e.ConfigureConsumer<PriceConsumer>(provider);
                    });
                }));

                x.AddConsumer<OrderConsumer>();
                x.AddConsumer<PriceConsumer>();
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
