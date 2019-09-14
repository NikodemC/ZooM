using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZooM.Api.Middleware;
using ZooM.Application;
using ZooM.Infrastructure;

namespace ZooM.Api
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
            services.AddCors(c =>
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()));

            services.RegisterApplication();
            services.RegisterInfrastructure();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(option =>
            {
                option.AllowAnyOrigin();
                option.AllowAnyHeader();
                option.AllowAnyMethod();
            });
            app.UseMiddleware<DomainExceptionMiddleware>();
            app.UseMvc();
            app.UseInfrastructure();
        }
    }
}
