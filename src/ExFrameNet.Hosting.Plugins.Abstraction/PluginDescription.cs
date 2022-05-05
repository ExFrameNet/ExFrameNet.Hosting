namespace ExFrameNet.Hosting.Plugins.Abstraction;

public class PluginDescription
{
    public string Name { get; }
    public Version Version { get; }
    public Type Type { get; }

    public PluginDescription(string name, Type type, Version version)
    {
        Name = name;
        Version = version;
        Type = type;
    }
}