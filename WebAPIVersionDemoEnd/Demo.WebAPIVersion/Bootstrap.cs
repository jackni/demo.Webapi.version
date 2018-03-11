using Demo.DataAccess;
using Demo.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.WebAPIVersion
{
    public static class Bootstrap
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.AddSingleton(new LoggerFactory().AddConsole());

            // add logging
            serviceCollection.AddLogging();

            //Versioning
            var versioningSection = config.GetSection("version");
            serviceCollection.AddOptions()
                .Configure<AppVersion>(versioningSection);

            //Ef core contenxt 
            serviceCollection.AddDbContext<DemoContext>(options =>
                options.UseSqlServer(config.GetConnectionString("demoDb")));

            //pre-request.
            serviceCollection.AddScoped<ITodoRepository, TodoRepository>();

            //transint
            serviceCollection.AddTransient<ISimpleMath, SimpleMath>();
            serviceCollection.AddTransient<ISimpleMathV2, SimpleMath>();

            serviceCollection.AddMvc();

            //enable api version
            serviceCollection.AddApiVersioning(v =>
            {
                v.ReportApiVersions = true;
                v.AssumeDefaultVersionWhenUnspecified = true;
                v.DefaultApiVersion = new ApiVersion(1, 0);
                //v.ApiVersionReader = new HeaderApiVersionReader("api-version"); //This will add to http heading
            });

            // Register the Swagger generator, defining one or more Swagger documents
            serviceCollection.AddSwaggerGen(c =>
            {
                var appVersion = new AppVersion();
                versioningSection.Bind(appVersion);
                var swaggerDocInfo = new Info { Title = "WebApi Demo", Version = appVersion.FullVersion };
                //grouping
                c.SwaggerDoc($"Math 1.0", swaggerDocInfo);
                c.SwaggerDoc($"Math 2.0", swaggerDocInfo);
                c.SwaggerDoc($"Todo demo", swaggerDocInfo);
            });
        }
    }
}
