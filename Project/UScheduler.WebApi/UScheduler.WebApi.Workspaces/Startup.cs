using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using UScheduler.WebApi.Workspaces.Data;
using UScheduler.WebApi.Workspaces.Interfaces;
using UScheduler.WebApi.Workspaces.Services;

namespace UScheduler.WebApi.Workspaces
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
            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UScheduler.WebApi.Workspaces", Version = "v1" });
            });

            services.AddScoped<IWorkspacesService, WorkspacesService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<WorkspacesContext>(
                            options => options.UseSqlServer(Configuration.GetConnectionString("WorkspacesDB")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UScheduler.WebApi.Workspaces v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (env.IsProduction())
            {
                InitializeDatabase(app);
            }
        }

        private static void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app?.ApplicationServices?.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                scope?.ServiceProvider.GetRequiredService<WorkspacesContext>().Database.Migrate();
            }
        }
    }
}
