using Console.Infrastructure.Identity;
using Console.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Console.WebUI;

public class Program {
    public static async Task Main(string[] args) {
        IHost? host = CreateHostBuilder(args).Build();

        using (IServiceScope scope = host.Services.CreateScope()) {
            IServiceProvider services = scope.ServiceProvider;

            try {
                ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

                if (context.Database.IsSqlServer())
                    context.Database.Migrate();

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                await ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager);
                await ApplicationDbContextSeed.SeedSampleDataAsync(context);
            } catch (Exception ex) {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                throw;
            }
        }

        await host.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
                webBuilder.UseStartup<Startup>());
    }
}
