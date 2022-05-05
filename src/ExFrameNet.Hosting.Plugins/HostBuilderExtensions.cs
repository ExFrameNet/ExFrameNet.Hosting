using ExFrameNet.Hosting.Plugins.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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

    public static IHostBuilder LoadPlugins(this IHostBuilder builder)
    {

        var plugins = GetPluginCollection(builder);
        var manager = new PluginManager(plugins);
        builder.ConfigureServices((context,services) =>
        {
            if(services.Any(d => d.ImplementationType == typeof(PluginManager)))
            {
                throw new InvalidOperationException("plugins already Loaded");
            }
            services.AddSingleton<IPluginManager>(manager);
            manager.LoadPlugins(services);
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
}
