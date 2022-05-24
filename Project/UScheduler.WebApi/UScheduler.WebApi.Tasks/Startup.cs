using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using UScheduler.WebApi.Tasks.Adapters;
using UScheduler.WebApi.Tasks.Data;
using UScheduler.WebApi.Tasks.Interfaces;
using UScheduler.WebApi.Tasks.Interfaces.Task;
using UScheduler.WebApi.Tasks.Interfaces.ToDo;
using UScheduler.WebApi.Tasks.Repositories;
using UScheduler.WebApi.Tasks.Services;

namespace UScheduler.WebApi.Tasks
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UScheduler.WebApi.Tasks", Version = "v1" });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<ITasksRepository, TasksRepository>();
            services.AddScoped<ITasksService, TasksService>();
            services.AddScoped<IToDoRepository, ToDoRepository>();
            services.AddScoped<IToDoService, ToDoService>();
            services.AddHttpClient<IBoardsAdapter, BoardsAdapter>();

            services.AddDbContext<TasksContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("TasksDB"));
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
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UScheduler.WebApi.Tasks v1"));
            

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
                scope?.ServiceProvider.GetRequiredService<TasksContext>().Database.Migrate();
            }
        }
    }
}
