namespace ExFrameNet.Hosting.Plugins.Abstraction;

public interface IPluginLoader
{
    Type PluginType { get; }
    void LoadPlugin(IPlugin plugin, IServiceProvider serviceProvider);
}