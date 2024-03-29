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
using Mikro.Messages.Commands;
using RawRabbit;
using RawRabbit.Instantiation;

namespace Mikro.Service
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
            services.AddSingleton<ICalculate,Calculate>();
            services.AddTransient<ICommandHandler<CalculateValueCommand>, CalculateValueHandler>();

            /* var options = new RawRabbitOptions();
            var section = Configuration.GetSection("rabbitmq");
            section.Bind(options);
            
            services.AddRawRabbit(options);  */



            var options = new RawRabbitOptions();
            var section = Configuration.GetSection("rabbitmq");
            section.Bind(options);


            var client = RawRabbitFactory.CreateSingleton(options);
            services.AddSingleton<IBusClient>(_ => client);

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
            var handler = app.ApplicationServices.GetService<ICommandHandler<CalculateValueCommand>>();
            client.SubscribeAsync<CalculateValueCommand>(msg => handler.HandleAsync(msg));

        }
    }
}
