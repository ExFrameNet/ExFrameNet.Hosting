using ExFrameNet.Hosting.Plugins.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace ExFrameNet.Hosting.Plugins;

public sealed class PluginManager : IPluginManager
{
    Dictionary<PluginDescription, IPlugin> _plugins = new();

    public Predicate<PluginDescription> LoadingCondition { get; set; } = _ => true;
    public IEnumerable<PluginDescription> LoadedPlugins => _plugins.Keys;
    public IPluginCollection DiscoverdPlugins { get; }
    public IPluginLoaderCollection PluginLoaders { get; }

    public event EventHandler<PluginLoadedEventArgs>? PluginLoaded;

    public PluginManager(IPluginCollection discoveredPlugins, IPluginLoaderCollection pluginLoaders)
    {
        DiscoverdPlugins = discoveredPlugins;
        PluginLoaders = pluginLoaders;
    }



    private void OnPluginLoaded(PluginDescription description)
    {
        PluginLoaded?.Invoke(this, new PluginLoadedEventArgs(description));
    }

    public void LoadPlugins(IServiceProvider services)
    {
        foreach (var (plugindescription, plugin) in _plugins)
        {
            var loader = PluginLoaders.GetLoaderFor(plugin);
            loader.LoadPlugin(plugin, services);
            OnPluginLoaded(plugindescription);
        }
    }

    public void RegisterPlugins(IServiceCollection services)
    {
        foreach (var plugin in DiscoverdPlugins)
        {
            if (LoadingCondition(plugin))
            {
                if (Activator.CreateInstance(plugin.Type) is not IPlugin pluginInstance)
                {
                    continue;
                }
                pluginInstance.RegisterServices(services);
                _plugins.Add(plugin, pluginInstance);
            }
        }
    }
}