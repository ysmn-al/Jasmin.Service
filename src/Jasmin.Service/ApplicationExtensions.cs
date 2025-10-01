//using Blazorise;
//using Blazorise.Bootstrap5;
//using Blazorise.Icons.FontAwesome;
using Jasmin.Service.Clients.Jasmin.Host.Client;
using Jasmin.Service.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Radzen;
using System;
using System.Net.Http.Headers;
using System.Net.Mime;

namespace Jasmin.Service;

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
            .AddTransient<IApplicationService, ApplicationService>()
            .AddScoped<AuthenticationStateProviderService>()
            .AddScoped<IAuthenticationService, AuthenticationService>();

        //services
        //    .AddBlazorise(options =>
        //    {
        //        options.Immediate = true;
        //    })
        //    .AddBootstrap5Providers()
        //    .AddFontAwesomeIcons();

        services.AddRadzenComponents();

        return services;
    }

    /// <summary>
    /// Добавить зависимости интерграции
    /// </summary>
    public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettings = new ApplicationConfig();
        configuration.Bind(appSettings);
        services.AddSingleton(Options.Create(appSettings));

        services.AddHttpClient<IJasminClient, JasminClient>()
            .ConfigureHttpClient((sp, client) =>
            {
                client.BaseAddress = new Uri(appSettings.JasminHostUrl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            });

        return services;
    }

}
