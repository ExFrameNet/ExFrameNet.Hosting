namespace ExFrameNet.Hosting.Plugins.Abstraction;
public interface IPluginCollection: IEnumerable<PluginDescription>
{
    void Add<T>() where T : IPlugin;
    void Add(Type pluginType);
    void Add(PluginDescription pluginDescription);
    void AddFromDirectory(string directory);
}
