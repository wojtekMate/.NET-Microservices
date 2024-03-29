﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mikro.Api.Repositories;
using Mikro.Messages.Events;
using RawRabbit;
using RawRabbit.Instantiation;


namespace Mikro.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            /* var builder = new ConfigurationBuilder().AddConfiguration(Configuration);
            services.AddRawRabbit(cfg =>cfg.AddJsonFile("rawrabbit.json")); */


            /* services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            var options = new RawRabbitOptions();
            var section = Configuration.GetSection("rabbitmq");
            section.Bind(options);
            
            services.AddRawRabbit(options);
            var service = new ServiceCollection()
            .AddRawRabbit<CustomContext>()
            .BuildServiceProvider();
            var client = service.GetService<IBusClient<CustomContext>>(); */


            var options = new RawRabbitOptions();
            var section = Configuration.GetSection("rabbitmq");
            section.Bind(options);
            var client = RawRabbitFactory.CreateSingleton(options);
            services.AddSingleton<IBusClient>(_ => client);
            services.AddSingleton<IRepository,InMemoryRepository>();
            services.AddTransient<IEventHandler<ValueCalculatedEvent>, ValueCalculatedHandler>();

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            ConfigureRabbitMqSubscriptions(app);
        }

        private void ConfigureRabbitMqSubscriptions(IApplicationBuilder app)
        {
            IBusClient client = app.ApplicationServices.GetService<IBusClient>();
            var handler = app.ApplicationServices.GetService<IEventHandler<ValueCalculatedEvent>>();
            client.SubscribeAsync<ValueCalculatedEvent>(msg => handler.HandleAsync(msg));

        }
    }
}
