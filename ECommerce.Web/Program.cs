
using ECommerce.Domain.Contracts;
using ECommerce.Domain.Entities.IdentityModule;
using ECommerce.Persistance.Data.DataSeed;
using ECommerce.Persistance.Data.DbContexts;
using ECommerce.Persistance.IdentityData.DbContexts;
using ECommerce.Persistance.Repositories;
using ECommerce.Presentation.Controllers;
using ECommerce.Services;
using ECommerce.Services.MappingProfiles;
using ECommerce.ServicesAbstarction;
using ECommerce.Web.CustomMiddleWares;
using ECommerce.Web.Extentions;
using ECommerce.Web.Factories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddKeyedScoped<IDataInitializer, DataIntializer>("Default");
            builder.Services.AddKeyedScoped<IDataInitializer, IdentityDataIntializer>("Identity");
            builder.Services.AddAutoMapper(X => X.AddProfile<ProductProfile>());
            builder.Services.AddAutoMapper(X => X.AddProfile<BasketProfile>());
            builder.Services.AddAutoMapper(X => X.AddProfile<OrderProfile>());
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductsServices, ProductServices>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(o =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")!);
            });
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketServices, BasketServices>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheServices, CacheServices>();
            builder.Services.Configure<ApiBehaviorOptions>(options => 
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.GenerateApiValidationresponse;
            });
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(ProductsController).Assembly);
            builder.Services.AddDbContext<StoreIdentityDbContext>(options => 
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<StoreIdentityDbContext>();
            builder.Services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Auth
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // UnAuth
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["JWTOptions:Issuer"],
                    ValidAudience = builder.Configuration["JWTOptions:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTOptions:SecretKey"]!))
                };
            });

            builder.Services.AddScoped<IOrderServices, OrderServices>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            #endregion

            var app = builder.Build();

            #region Seed Data

            await app.MigrateDbAsync();
            await app.MigrateIdentityDbAsync();
            await app.SeedDataAsync();
            await app.SeedIdentityDataAsync();

            #endregion


            // Configure the HTTP request pipeline.
            // Exception handling
            app.UseMiddleware<ExceptionHandlerMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
