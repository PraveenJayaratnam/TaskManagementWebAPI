using Microsoft.OpenApi.Models;
using System.Text.Json;
using Application.Common;
using Infrastructure.Data;
using Application.Mappings;
using Infrastructure.UnitOfWork;
using Application.Services;
using Application.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace TaskManagementWebAPI;

public static class StartupConfiguration
{
    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            });

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IAuditService, AuditService>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddSingleton(provider =>
        {
            var config = new TypeAdapterConfig();
            config.Scan(typeof(MappingProfile).Assembly);
            return config;
        });

        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);
        services.AddFluentValidationAutoValidation();

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Management API", Version = "v1" });
        });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp", policy =>
            {
                policy.WithOrigins("http://localhost:4200")
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        services.AddAuthentication("CustomScheme")
            .AddScheme<AuthenticationSchemeOptions, CustomAuthenticationHandler>("CustomScheme", options => { });

        services.AddAuthorization();

        return services;
    }

    public static void LogApplicationStartup(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Task Management API starting up...");
    }
}
