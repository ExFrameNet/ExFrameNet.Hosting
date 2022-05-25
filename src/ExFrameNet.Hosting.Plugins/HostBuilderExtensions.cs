using ExFrameNet.Hosting.Plugins.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExFrameNet.Hosting.Plugins;
public static class HostBuilderExtensions
{
    public static IHostBuilder ConfigurePlugins(this IHostBuilder builder, Action<IPluginCollection> configuration)
    {
        var pluginCollection = GetPluginCollection(builder);
        configuration(pluginCollection);

        return builder;
    }

    public static IHostBuilder ConfigurePluginLoaders(this IHostBuilder builder, Action<IPluginLoaderCollection> configuration)
    {
        var pluginLoaderCollection = GetPluginLoaders(builder);
        configuration(pluginLoaderCollection);
        return builder;
    }

    public static IHostBuilder LoadPlugins(this IHostBuilder builder)
    {

        var plugins = GetPluginCollection(builder);
        var pluginLoaders = GetPluginLoaders(builder);
        var manager = new PluginManager(plugins, pluginLoaders);
        builder.ConfigureServices((context,services) =>
        {
            if(services.Any(d => d.ImplementationType == typeof(PluginManager)))
            {
                throw new InvalidOperationException("plugins already Loaded");
            }
            services.AddSingleton<IPluginManager>(manager);
            manager.RegisterPlugins(services);
        });

        return builder;
    }


    public static IHostBuilder ConfigureAndLoadPlugins(this IHostBuilder builder, Action<IPluginCollection> configuration)
    {
        ConfigurePlugins(builder, configuration);
        return LoadPlugins(builder);
    }

    private static IPluginCollection GetPluginCollection(IHostBuilder builder)
    {
        if (!builder.Properties.TryGetValue("PluginCollection", out var pluginCollection))
        {
            pluginCollection = new PluginCollection();
            builder.Properties["PluginCollection"] = pluginCollection;
        }

        return (IPluginCollection)pluginCollection;
    }

    private static IPluginLoaderCollection GetPluginLoaders(IHostBuilder builder)
    {
        if (!builder.Properties.TryGetValue("PluginLoaderCollection", out var pluginLoaderCollection))
        {
            pluginLoaderCollection = new PluginLoaderCollection();
            ((IPluginLoaderCollection)pluginLoaderCollection).Add(new DefaultPluginLoader());
            builder.Properties["PluginLoaderCollection"] = pluginLoaderCollection;

        }

        return (IPluginLoaderCollection)pluginLoaderCollection;
    }
}
