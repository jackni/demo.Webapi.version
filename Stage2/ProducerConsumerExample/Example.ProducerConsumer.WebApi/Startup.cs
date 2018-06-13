using System;
using System.IO;
using Example.ProducerConsumer.WebApi.Infrastructure;
using Example.Settings;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Example.ProducerConsumer.WebApi
{
    public class Startup
    {
		public IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration, IHostingEnvironment env)
		{
			Configuration =
				new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
					.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
					.AddEnvironmentVariables()
					.Build();
		}

		// This method gets called by the runtime. Use this method to add services to the container.		
		public void ConfigureServices(IServiceCollection services)
		{
			Bootstrap.ConfigureServices(services, Configuration);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app,
			IHostingEnvironment env,
			IServiceProvider serviceProvider,
			IApplicationLifetime applicationLifetime,
			ILoggerFactory loggerFactory)
        {
			try
			{
				if (env.IsDevelopment())
				{
					app.UseDeveloperExceptionPage();
				}

				var applicationSettings = serviceProvider.GetService<IOptions<ApplicationSettings>>().Value;
				if (applicationSettings.WebapiEnable)
				{
					// Enable middle-ware to serve generated Swagger as a JSON endpoint.
					app.UseSwagger();
					// Enable middle-ware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
					app.UseSwaggerUI(c =>
					{
						c.SwaggerEndpoint("/swagger/SimpleCommand/swagger.json", $"Demo SimpleCommand API");						
					});

					app.UseExceptionMiddleware();

					app.UseMvc();
				}

				if (applicationSettings.MessageBusEnable)
				{
					//trigger bus start.
					var bus = serviceProvider.GetService<IBusControl>();
					applicationLifetime.ApplicationStarted.Register(() => TaskUtil.Await(() => bus.StartAsync()));
					applicationLifetime.ApplicationStopping.Register(() => TaskUtil.Await(() => bus.StopAsync()));
				}
			}
			catch(Exception ex)
			{
				var baseEx = ex.GetBaseException();
				var logger = serviceProvider.GetService<ILogger<Startup>>();
				logger.LogCritical($"###Application FATAL Error: {baseEx.Message} ###");
#if DEBUG
				Console.ReadLine();
#endif
				throw;
			}

        }
    }
}
