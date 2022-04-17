using System.Net;
using Console.Application;
using Console.Application.Common.Interfaces;
using Console.Infrastructure;
using Console.Infrastructure.Extensions;
using Console.Infrastructure.Persistence;
using Console.WebUI.Filters;
using Console.WebUI.Hubs;
using Console.WebUI.Models;
using Console.WebUI.Services;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Console.WebUI;

public class Startup {
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }


    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
        services.AddApplication();
        services.AddInfrastructure(Configuration);

        var spaSrcPath = "ClientApp";
        var corsPolicyName = "AllowAll";

        services.AddHealthChecks()
            .AddGcInfoCheck("GCInfo");

        services.AddHealthChecks();
        // .AddInMemoryStorage();


        services.AddCorsConfig(corsPolicyName);
        services.AddControllers(config => {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
            config.Filters.Add(new AuthorizeFilter(policy));
        });
        services.AddSignalR();
        services.AddMvc(opt => opt.SuppressAsyncSuffixInActionNames = false);
        
        ConnectionConfig connConfig = new ConnectionConfig();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllersWithViews(options =>
                options.Filters.Add<ApiExceptionFilterAttribute>())
            .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

        services.AddRazorPages();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        // In production, the Angular files will be served from this directory
        services.AddSpaStaticFiles(configuration =>
            configuration.RootPath = $"{spaSrcPath}/dist");

        services.AddOpenApiDocument(configure => {
            configure.Version = "v1";
            configure.Title = "Console API";
            configure.Description = "Detailed Description of API";
            configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme {
                Type = OpenApiSecuritySchemeType.ApiKey,
                Name = "Authorization",
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "Type into the textbox: Bearer {your JWT token}."
            });

            configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        } else {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            // app.UseResponseCompression();
        }

        // app.UseCors("AllowAll");

        app.UseExceptionHandler(builder => {
            builder.Run(async context => {
                IExceptionHandlerFeature? error = context.Features.Get<IExceptionHandlerFeature>();
                ExceptionDetails exDetails = new((int)HttpStatusCode.InternalServerError, error?.Error.Message ?? "");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = exDetails.StatusCode;
                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.Headers.Add("Application-Error", exDetails.Message);
                context.Response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");

                await context.Response.WriteAsync(exDetails.ToString());
            });
        });

        app.UseHealthChecksUI();

        app.UseHealthChecks("/healthchecks-json", new HealthCheckOptions {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        if (!env.IsDevelopment())
            app.UseSpaStaticFiles();

        app.UseOpenApi();
        app.UseSwaggerUi3(settings => {
            settings.Path = "/api";
            settings.DocumentPath = "/api/specification.json";
            settings.CustomStylesheetPath = "/SwaggerDark.css";
            System.Console.WriteLine(settings.CustomStylesheetPath);
            
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseIdentityServer();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
            endpoints.MapHub<UsersHub>("/hubs/users");
        });

        app.UseSpa(spa => {
            // To learn more about options for serving an Angular SPA from ASP.NET Core,
            // see https://go.microsoft.com/fwlink/?linkid=864501

            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment()) spa.UseReactDevelopmentServer("start");
            // }
        });
    }
}
