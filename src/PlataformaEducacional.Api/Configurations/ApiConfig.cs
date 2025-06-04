using Microsoft.EntityFrameworkCore;
using PlataformaEducacional.Aluno.Data;
using PlataformaEducacional.Api.Data;
using PlataformaEducacional.Conteudo.Data;
using PlataformaEducacional.Financeiro.Data;

namespace PlataformaEducacional.Api.Configurations
{
    public static class ApiConfig
    {
        public static void AddApiConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Context>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<AlunoContext>(options =>
               options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<ConteudoContext>(options =>
               options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddDbContext<FinanceiroContext>(options =>
              options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                         .WithOrigins("http://localhost:4200")
                         .AllowAnyMethod()
                         .AllowAnyHeader()
                         .AllowCredentials()
                         .WithExposedHeaders("X-Pagination"));
            });

            services.AddSwaggerGen();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerConfig();
            services.AddAutoMapper(typeof(Program));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
        }

        public static void UseApiConfig(this WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerConfig();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Total");

            app.UseIdentityConfig();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
