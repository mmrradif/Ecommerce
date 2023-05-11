using EcommerceInterfaces;
using EcommerceInterfaces.Auth;
using EcommerceModels;
using EcommerceModels.Auth;
using EcommerceModels.HostedService;
using EcommerceRepositories;
using EcommerceRepositories.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Ecommerce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            

            // For Migration
            builder.Services.AddDbContext<EcommerceDbContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("AppCon")
                   );
            });


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            builder.Services.AddTransient<IAll<User>, UserRepo>();
            builder.Services.AddTransient<IUser, UserRepo>();
            builder.Services.AddTransient<IStoreProcedures<User>, UserRepo>();


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddHostedService<DbSeederHostedService>();


            // Add Cors
            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("EcommercePolicy", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });


            builder.Services.AddControllers()
             .AddNewtonsoftJson(option =>
             {
                 option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize;
                 option.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
             });


            // JWT
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
               
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;           
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {             
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("supersecret.....")),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };

            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

           
            // Use Cors
            app.UseCors("EcommercePolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();


            app.MapControllers();

            app.Run();
        }
    }
}