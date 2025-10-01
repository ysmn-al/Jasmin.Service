using Jasmin.Common.Services;
using Jasmin.Db;
using Jasmin.Db.DB;
using Jasmin.Service.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Jasmin.Host;

/// <summary>
/// Расширение для подключения сервисов
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Добавить зависимости слоя приложения
    /// </summary>
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        services
            .AddScoped<IAppService, AppService>()
            .AddScoped<InitialDataService>()
            ;
        return services;
    }

    /// <summary>
    /// Добавить зависимости слоя доступа к данным
    /// </summary>
    public static IServiceCollection AddDbDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStrings = new ConnectionStrings();
        configuration.GetSection("ConnectionStrings").Bind(connectionStrings);
        services.AddSingleton(Options.Create(connectionStrings));

        //services.AddDbContext<IJasminDBContext, JasminDBContext>(
        //    options => options.UseSqlServer(connectionStrings.ConnectionString, opts => opts.CommandTimeout(connectionStrings.TimeOut)));

        services.AddDbContext<IJasminDBContext, JasminDBContext>(
            options => options.UseNpgsql(connectionStrings.ConnectionString, opts => opts.CommandTimeout(connectionStrings.TimeOut)));

        return services;
    }

}
