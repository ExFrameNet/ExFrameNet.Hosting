using ExFrameNet.Hosting.Plugins.Abstraction;
using Microsoft.Extensions.DependencyInjection;

namespace ExFrameNet.Hosting.Plugins;

internal class PluginManager : IPluginManager
{
    Dictionary<PluginDescription, IPlugin> _plugins = new();

    public Predicate<PluginDescription> LoadingCondition { get; set; } = _ => true;
    public IEnumerable<PluginDescription> LoadedPlugins => _plugins.Keys;
    public IPluginCollection DiscoverdPlugins { get; }

    public event EventHandler<PluginInitilizedEventArgs>? PluginInitilized;

    public PluginManager(IPluginCollection discoveredPlugins)
    {
        DiscoverdPlugins = discoveredPlugins;
    }

    public void InitializePlugins(IServiceProvider services)
    {
        foreach (var (plugindescription, plugin) in _plugins)
        {
            plugin.Initialize(services);
            OnPluginInitilized(plugindescription);
        }
    }

    public void LoadPlugins(IServiceCollection services)
    {
        foreach (var plugin in DiscoverdPlugins)
        {
            if (LoadingCondition(plugin))
            {
                if(Activator.CreateInstance(plugin.Type) is not IPlugin pluginInstance)
                {
                    continue;
                }
                _plugins.Add(plugin, pluginInstance);
            }
        }
    }

    private void OnPluginInitilized(PluginDescription description)
    {
        PluginInitilized?.Invoke(this, new PluginInitilizedEventArgs(description));
    }
}