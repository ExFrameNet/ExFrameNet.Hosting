using ExFrameNet.Hosting.Plugins.Abstraction;
using System.Collections;

namespace ExFrameNet.Hosting.Plugins;
public class PluginLoaderCollection : IPluginLoaderCollection
{
    private readonly Dictionary<Type, IPluginLoader> _loaders = new Dictionary<Type, IPluginLoader>();

    public void Add(IPluginLoader loader)
    {
        _loaders.Add(loader.PluginType, loader);
    }

    public void AddFromCollection(IPluginLoaderCollection collection)
    {
        foreach (var loader in collection)
        {
            _loaders.TryAdd(loader.PluginType, loader);
        }
    }


    public IPluginLoader GetLoaderFor<T>() where T : IPlugin
    {
        return GetLoaderFor(typeof(T));
    }

    public IPluginLoader GetLoaderFor(Type pluginType)
    {
        if(!pluginType.IsAssignableTo(typeof(IPlugin)))
        {
            throw new NotSupportedException($"Plugin must implement the {typeof(IPlugin)} interface");
        }

        var baseType = pluginType;
        while (baseType != null)
        {
            if (_loaders.ContainsKey(baseType))
            {
                return _loaders[baseType];
            }

            baseType = baseType.BaseType;
        }

        foreach (var @interface in pluginType.GetInterfaces())
        {
            if (_loaders.ContainsKey(@interface))
            {
                return _loaders[@interface];
            }
        }
        throw new KeyNotFoundException($"No pluginloader for {pluginType} found");
    }

    public IPluginLoader GetLoaderFor(IPlugin plugin)
    {
        return GetLoaderFor(plugin.GetType());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _loaders.Values.GetEnumerator();
    }
    public IEnumerator<IPluginLoader> GetEnumerator()
    {
        return _loaders.Values.GetEnumerator();
    }
}
