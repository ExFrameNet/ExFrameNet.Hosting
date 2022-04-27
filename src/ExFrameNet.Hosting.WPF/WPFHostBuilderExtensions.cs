using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Windows;

namespace ExFrameNet.Hosting.WPF;

public static class WPFHostBuilderExtensions
{
    public static WPFHostBuilder WithApp<TApp>(this WPFHostBuilder builder)
        where TApp : Application
    {
        builder.Services.TryAddSingleton<Application, TApp>();
        return builder;
    }

    public static WPFHostBuilder WithAppFactory<TApp>(this WPFHostBuilder builder, Func<IServiceProvider, TApp> factory)
        where TApp : Application
    {
        builder.Services.TryAddSingleton<Application>(factory);
        return builder;
    }

    public static WPFHostBuilder AddResources(this WPFHostBuilder builder, ResourceDictionary resources)
    {
        builder.ConfigureResources(r => r.MergedDictionaries.Add(resources));
        return builder;
    }

    public static WPFHostBuilder AddResources(this WPFHostBuilder builder, string path)
    {
        builder.ConfigureResources(r =>
        {
            var res = new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/" + path)
            };
            r.MergedDictionaries.Add(res);
        });
        
        return builder;
    }
}
