namespace ExFrameNet.Hosting.Plugins.Abstraction.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public class PluginNameAttribute : Attribute
{
    public string Name { get; }

    public PluginNameAttribute(string name)
    {
        Name = name;
    }
}
