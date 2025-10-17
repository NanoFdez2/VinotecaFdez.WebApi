using Microsoft.EntityFrameworkCore;
using Vinoteca.Abstractions;
using Vinoteca.Applications;
using Vinoteca.DataAccess;
using Vinoteca.Repositories;
using Vinoteca.Services;

namespace VinotecaFernandez.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped(typeof(IStringServices), typeof(StringServices));
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IApplication<>), typeof(Application<>));
            builder.Services.AddScoped(typeof(IDbContext<>), typeof(DbContext<>));

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<DbDataAccess>(options =>
            {
                // Corrección: nombre correcto del ensamblado donde publicar las migraciones
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly("VinotecaFernandez.WebApi");
                    sqlOptions.EnableRetryOnFailure();
                });
                options.UseLazyLoadingProxies();
            });

            builder.Services.AddAutoMapper(typeof(Program));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
