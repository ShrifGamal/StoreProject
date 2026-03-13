using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Store.APIs.Errors;
using Store.Core;
using Store.Core.Mapping.Baskets;
using Store.Core.Mapping.Products;
using Store.Core.RepositoriesContract;
using Store.Core.ServicesContract;
using Store.Repository;
using Store.Repository.Data.Contexts;
using Store.Repository.Identity.Contexts;
using Store.Repository.Repositories;
using Store.Service.Services.Caches;
using Store.Service.Services.Products;
using Store.Service.Services.Token;
using Store.Service.Services.Users;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Store.Core.Entites.Identity;
using Store.Core.Mapping.Auth;
using Store.Core.Mapping.Orders;
using Store.Service.Services.Basket;
using Store.Service.Services.Orders;
using Store.Service.Services.Payments;

namespace Store.APIs.Helper
{
    public static class DependencyInJection
    {

        public static IServiceCollection AddDependency(this IServiceCollection Services , IConfiguration configuration)
        {
            Services.AddBuiltInService();
            Services.AddSwaggerService();
            Services.AddContextService(configuration);
            Services.AddUserDeFinedService();
            Services.AddAutoMapperService(configuration);
            Services.ConfigerInvaliedModelStateResponseService();
            Services.AddRedisService(configuration);
            Services.AddIdentityService();
            Services.AddAuthenticationService(configuration);


            return Services;
        }
        private static IServiceCollection AddBuiltInService(this IServiceCollection Services)
        {
            Services.AddControllers();

            return Services;
        }
        private static IServiceCollection AddSwaggerService(this IServiceCollection Services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();
            return Services;
        }
        private static IServiceCollection AddContextService(this IServiceCollection Services , IConfiguration configuration)
        {
            Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            Services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            return Services;
        }
        private static IServiceCollection AddUserDeFinedService(this IServiceCollection Services)
        {
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<ICacheService, CacheService>();
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IBasketcRepository, BasketcRepository>();
            Services.AddScoped<IBasketService, BasketService>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IPaymentService, PaymentService>();

            return Services;
        }
        private static IServiceCollection AddAutoMapperService(this IServiceCollection Services , IConfiguration configuration)
        {
            Services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            Services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            Services.AddAutoMapper(M => M.AddProfile(new AuthProfile()));
            Services.AddAutoMapper(M => M.AddProfile(new OrderProfile(configuration)));

            return Services;
        }
        private static IServiceCollection ConfigerInvaliedModelStateResponseService(this IServiceCollection Services)
        {
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var error = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                            .SelectMany(P => P.Value.Errors)
                                            .Select(E => E.ErrorMessage)
                                            .ToArray();

                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = error
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return Services;
        }
        private static IServiceCollection AddRedisService(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
            {
                var connection = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });

            return Services;
        }
        private static IServiceCollection AddIdentityService(this IServiceCollection Services)
        {
            Services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<StoreIdentityDbContext>();

            return Services;
        }
        private static IServiceCollection AddAuthenticationService(this IServiceCollection Services , IConfiguration configuration)
        {
            Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });

            return Services;
        }

    }
}
