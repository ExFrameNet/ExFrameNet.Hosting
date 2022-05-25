namespace ExFrameNet.Hosting.Plugins.Abstraction;

public interface IPluginLoaderCollection : IEnumerable<IPluginLoader>
{
    void Add(IPluginLoader loader);
    void AddFromCollection(IPluginLoaderCollection collection);

    IPluginLoader GetLoaderFor<T>()
        where T : IPlugin;

    IPluginLoader GetLoaderFor(Type pluginType);
    IPluginLoader GetLoaderFor(IPlugin plugin);
}