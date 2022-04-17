using Console.Application.Common.Interfaces;
using Console.Infrastructure.Identity;
using Console.Infrastructure.Persistence;
using Console.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Console.Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        // if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            // services.AddDbContext<ApplicationDbContext>(options =>
                // options.UseInMemoryDatabase("ConsoleDb"));
        // else {

            ConnectionConfig connConfig = new ConnectionConfig();
            configuration.GetSection("DevConnection").Bind(connConfig);
            var cnnString = new NpgsqlConnectionStringBuilder() {
                Host = connConfig.Host,
                Port = connConfig.Port,
                Database = connConfig.Database,
                Username = connConfig.Username,
                Password = connConfig.Password
            };

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    builder => 
                        builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IDomainEventService, DomainEventService>();

        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        // services.AddIdentityServer()
            // .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddTransient<IDateTime, DateTimeService>();
        // services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IDontCare, DontCare>();


        // services.AddAuthorization(options =>
            // options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));
        
        return services;
    }
}

public class ConnectionConfig {
    public string Host { get; set; }
    public int Port { get; set; }
    public string Database { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}
