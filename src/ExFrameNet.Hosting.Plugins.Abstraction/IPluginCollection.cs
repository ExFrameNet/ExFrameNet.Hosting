namespace ExFrameNet.Hosting.Plugins.Abstraction;
public interface IPluginCollection: IEnumerable<PluginDescription>
{
    void Add<T>() where T : IPlugin;
    void Add(Type pluginType);
    void AddFromDirectory(string directory);
}
