using ExFrameNet.Hosting.Plugins.Abstraction;

namespace ExFrameNet.Hosting.Plugins;
public abstract class PluginLoader<T> : IPluginLoader
    where T : IPlugin
{
    public Type PluginType => typeof(T);

    public void LoadPlugin(IPlugin plugin, IServiceProvider serviceProvider)
    {
        LoadPlugin((T)plugin, serviceProvider);
    }

    public abstract void LoadPlugin(T Plugin, IServiceProvider serviceProvider);
}
