using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using UScheduler.WebApi.Boards.Adapters;
using UScheduler.WebApi.Boards.Data;
using UScheduler.WebApi.Boards.Interfaces;
using UScheduler.WebApi.Boards.Repositories;
using UScheduler.WebApi.Boards.Services;

namespace UScheduler.WebApi.Boards
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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UScheduler.WebApi.Boards", Version = "v1" });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IBoardsRepository, BoardsRepository>();
            services.AddScoped<IBoardsService, BoardsService>();

            services.AddHttpClient<IWorkspacesAdapter, WorkspacesAdapter>();

            services.AddDbContext<BoardsContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("BoardsDB"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UScheduler.WebApi.Boards v1"));

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
                scope?.ServiceProvider.GetRequiredService<BoardsContext>().Database.Migrate();
            }
        }
    }
}
