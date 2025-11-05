using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Vinoteca.Abstractions;
using Vinoteca.Applications;
using Vinoteca.DataAccess;
using Vinoteca.Entities.MicrosoftIdentity;
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

            builder.Services.AddDbContext<DbDataAccess>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                        o => o.MigrationsAssembly("VinotecaFdez.WebApi"));
                options.UseLazyLoadingProxies();
            });

            builder.Services.AddIdentity<User, Role>(
                options => options.SignIn.RequireConfirmedAccount = true).
                AddDefaultTokenProviders().
                AddEntityFrameworkStores<DbDataAccess>().
                AddSignInManager<SignInManager<User>>().
                AddRoleManager<RoleManager<Role>>();

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped(typeof(IStringServices), typeof(StringServices));
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IApplication<>), typeof(Application<>));
            builder.Services.AddScoped(typeof(IDbContext<>), typeof(DbContext<>));




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
