using Collectio.Infra.CrossCutting.Ioc;
using Collectio.Infra.CrossCutting.Services;
using Collectio.Presentation.Filters;
using Collectio.Presentation.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;

namespace Collectio.Presentation
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
            services.AddOData();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers(opt =>
            {
                opt.Filters.Add<CustomExceptionFilter>();
            }).AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.Formatting = Formatting.Indented;
            });
            services.AddScoped<ITenantIdProvider, TenantIdProvider>(e =>
            {
                var httpContextAccessor = e.GetService<IHttpContextAccessor>();
                Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value, out Guid tenantId);
                return new TenantIdProvider(tenantId);
            });
            services.AddLogging();

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration.GetSection("IdentityServer:Authority").Value;
                    options.RequireHttpsMetadata = Configuration.GetSection("IdentityServer").GetValue<bool>("RequireHttpsMetadata");
                    options.Audience = Configuration.GetSection("IdentityServer:ApiName").Value;
                });

            services.AddCors(e => e.AddPolicy("default",
                c => c.WithOrigins(Configuration.GetValue<string>("CollectioFrontUri")).AllowAnyHeader()
                    .AllowAnyMethod()));

            services.RegisterDependencies(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("default");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.Select().Filter().OrderBy().Count().MaxTop(25);
                endpoints.MapODataRoute("odata", "odata", ModelProvider.GetEdmModel());
            });
        }
    }
}
