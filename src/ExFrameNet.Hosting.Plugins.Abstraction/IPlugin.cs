using Microsoft.Extensions.DependencyInjection;

namespace ExFrameNet.Hosting.Plugins;
public interface IPlugin
{
    void RegisterServices(IServiceCollection services);

    void Initialize(IServiceProvider services);
}
