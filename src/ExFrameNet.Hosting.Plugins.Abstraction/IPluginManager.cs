using Microsoft.Extensions.DependencyInjection;

namespace ExFrameNet.Hosting.Plugins.Abstraction;

public interface IPluginManager
{

    event EventHandler<PluginInitilizedEventArgs>? PluginInitilized;
    
    Predicate<PluginDescription> LoadingCondition { get; set; }
    IEnumerable<PluginDescription> LoadedPlugins { get; }
    IPluginCollection DiscoverdPlugins { get; }

    void InitializePlugins(IServiceProvider services);
    void LoadPlugins(IServiceCollection services);
}
