using Infra.Ioc;

namespace RGRTRASPORTE
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
            services.AddAutoMapper(typeof(Startup));

            services.AddControllers();

            services.AddSingleton(Configuration);

            services.AddContext(Configuration.GetValue<string>("ConnectionStrings:RGRTRASPORTE"));

            services.AddSwagger();

            services.ConfigureApi();

            services.AddServices();

            services.AddRepositorys();
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Painel.Api v1"));

            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4200");
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
