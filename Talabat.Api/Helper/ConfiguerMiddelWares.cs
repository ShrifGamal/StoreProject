using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.APIs.MiddelWares;
using Store.Core.Entites.Identity;
using Store.Repository.Data;
using Store.Repository.Data.Contexts;
using Store.Repository.Identity;
using Store.Repository.Identity.Contexts;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Store.APIs.Helper
{
    public static class ConfiguerMiddelWares
    {
        public static async Task<WebApplication> ConfiguerMiddelWareAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var Services = scope.ServiceProvider;

            var context = Services.GetRequiredService<StoreDbContext>();
            var IdentityContext = Services.GetRequiredService<StoreIdentityDbContext>();
            var userManager = Services.GetRequiredService<UserManager<AppUser>>();
            var loggerFactory = Services.GetRequiredService<ILoggerFactory>();

            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);
                await IdentityContext.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAppUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "There Are A Problem During Apply Migration !");
            }

            app.UseMiddleware<ExceptionMiddelWare>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            return app;
        }
    }
}
