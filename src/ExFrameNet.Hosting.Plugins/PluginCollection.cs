using ExFrameNet.Hosting.Plugins.Abstraction;
using ExFrameNet.Hosting.Plugins.Abstraction.Attributes;
using System.Collections;
using System.Reflection;

namespace ExFrameNet.Hosting.Plugins;
internal class PluginCollection : IPluginCollection
{
    List<PluginDescription> _plugins = new();

    public void Add<T>() where T : IPlugin
    {
        var type = typeof(T);
        _plugins.Add(GetPluginDescription(type));
    }

    public void Add(Type pluginType)
    {
        if (!pluginType.GetInterfaces().Contains(typeof(IPlugin)))
        {
            throw new ArgumentException("pluginType must implement IPlugin");
        }

        _plugins.Add(GetPluginDescription(pluginType));
    }

    public void AddFromDirectory(string directory)
    {
        var innerDirectories = Directory.GetDirectories(directory);

        if (innerDirectories.Length == 0)
        {
            throw new InvalidOperationException("Plugins must be in a seperate folder per plugin");
        }

        foreach (var innerDirectory in innerDirectories)
        {
            var dirname = Path.GetFileName(Path.GetDirectoryName(innerDirectory));
            var file = Directory.GetFiles(innerDirectory, $"*{dirname}*.dll").First();

            if (file == null)
            {
                throw new FileNotFoundException($"no dll for '{dirname}' found");
            }

            var ass = Assembly.LoadFrom(file);
            var plugin = ass.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IPlugin))).First();

            if (plugin == null)
            {
                throw new Exception($"{file}' did not contain any Plugin");
            }

            Add(plugin);
        }
    }

    public IEnumerator<PluginDescription> GetEnumerator()
    {
        return _plugins.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _plugins.GetEnumerator();
    }

    private PluginDescription GetPluginDescription(Type type)
    {
        var name = type.Name;

        var nameAttribute = type.GetCustomAttribute<PluginNameAttribute>();
        if (nameAttribute != null)
        {
            name = nameAttribute.Name;
        }

        var version = type.Assembly.GetName().Version;
        var versionAtribute = type.GetCustomAttribute<PluginVersionAttribute>();
        if (versionAtribute != null)
        {
            version = versionAtribute.Version;
        }

        return new PluginDescription(name, type, version!);
    }
}
