using ExFrameNet.Hosting.Plugins.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExFrameNet.Hosting.Plugins;
internal class DefaultPluginLoader : IPluginLoader
{
    public Type PluginType => typeof(IPlugin);

    public void LoadPlugin(IPlugin plugin, IServiceProvider serviceProvider)
    {
        plugin.Initialize(serviceProvider);
    }
}
