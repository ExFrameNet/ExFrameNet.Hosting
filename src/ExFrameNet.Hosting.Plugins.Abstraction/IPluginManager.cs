using Microsoft.Extensions.DependencyInjection;

namespace ExFrameNet.Hosting.Plugins.Abstraction;

public interface IPluginManager
{
    event EventHandler<PluginLoadedEventArgs>? PluginLoaded;
    
    Predicate<PluginDescription> LoadingCondition { get; set; }
    IEnumerable<PluginDescription> LoadedPlugins { get; }
    IPluginCollection DiscoverdPlugins { get; }

    IPluginLoaderCollection PluginLoaders { get; }

    void LoadPlugins(IServiceProvider services);
    void RegisterPlugins(IServiceCollection services);
}
