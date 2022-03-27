using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Data.SqlClient;
using UScheduler.WebApi.Users.Data;
using UScheduler.WebApi.Users.Interfaces;
using UScheduler.WebApi.Users.Services;

namespace UScheduler.WebApi.Users
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "UScheduler.WebApi.Users", Version = "v1" });
            });
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IDataValidator, DataValidator>();

            services.AddSingleton(sp => GetSqlServerConnectionString());

            //services.AddDbContext<UsersContext>(
            //                options => options.UseSqlServer(Configuration.GetConnectionString("UsersDB")));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UScheduler.WebApi.Users v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            InitializeDatabase(app);
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app?.ApplicationServices?.GetService<IServiceScopeFactory>()?.CreateScope())
            {
                scope?.ServiceProvider.GetRequiredService<UsersContext>().Database.Migrate();
            }
        }

        public static SqlConnectionStringBuilder GetSqlServerConnectionString()
        {
            var connectionString = new SqlConnectionStringBuilder()
            {
                DataSource = Environment.GetEnvironmentVariable("DB_HOST"),
                UserID = Environment.GetEnvironmentVariable("DB_USER"),
                Password = Environment.GetEnvironmentVariable("DB_PASS"),
                InitialCatalog = Environment.GetEnvironmentVariable("DB_NAME"),
                Encrypt = false,
                Pooling = true,
                MaxPoolSize = 5,
                MinPoolSize = 0,
                ConnectTimeout = 15,
                TrustServerCertificate = true
            };

            return connectionString;
        }
    }
}
