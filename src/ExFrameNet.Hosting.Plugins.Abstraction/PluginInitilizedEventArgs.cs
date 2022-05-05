namespace ExFrameNet.Hosting.Plugins.Abstraction;

public class PluginInitilizedEventArgs
{
    public string PluginName { get; }
    public Version PluginVersion { get; }

    public PluginInitilizedEventArgs(string pluginName, Version pluginVersion)
    {
        PluginName = pluginName;
        PluginVersion = pluginVersion;
    }

    public PluginInitilizedEventArgs(PluginDescription description)
    {
        PluginName = description.Name;
        PluginVersion = description.Version;
    }
}