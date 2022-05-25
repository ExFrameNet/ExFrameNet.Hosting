namespace ExFrameNet.Hosting.Plugins.Abstraction;

public class PluginLoadedEventArgs
{
    public string PluginName { get; }
    public Version PluginVersion { get; }

    public PluginLoadedEventArgs(string pluginName, Version pluginVersion)
    {
        PluginName = pluginName;
        PluginVersion = pluginVersion;
    }

    public PluginLoadedEventArgs(PluginDescription description)
    {
        PluginName = description.Name;
        PluginVersion = description.Version;
    }
}