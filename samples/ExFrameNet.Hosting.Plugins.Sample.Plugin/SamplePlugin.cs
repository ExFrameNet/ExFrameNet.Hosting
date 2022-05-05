using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExFrameNet.Hosting.Plugins.Sample.Plugin;

public class SamplePlugin : IPlugin
{
    public void Initialize(IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILogger<SamplePlugin>>();
        logger.LogInformation("Plugin loaded");
    }

    public void RegisterServices(IServiceCollection services)
    {
        
    }
}
