using ExFrameNet.Hosting.Plugins.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExFrameNet.Hosting.Plugins.Sample;
internal class ConsoleService : IHostedService
{
    private readonly IServiceProvider _services;

    public ConsoleService(IServiceProvider services)
    {
        _services = services;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var pluginManager = _services.GetRequiredService<IPluginManager>();
        pluginManager.InitializePlugins(_services);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
