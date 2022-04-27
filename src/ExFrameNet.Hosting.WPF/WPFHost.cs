using Microsoft.Extensions.Hosting;

namespace ExFrameNet.Hosting.WPF;
public sealed class WPFHost : IHost, IAsyncDisposable
{
    private readonly IHost _innerHost;

    public IServiceProvider Services => _innerHost.Services;

    internal WPFHost(IHost innerHost)
    {
        _innerHost = innerHost;
    }

    

    public Task StartAsync(CancellationToken cancellationToken = default)
    {
        return _innerHost.StartAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken = default)
    {
        return _innerHost.StopAsync(cancellationToken);
    }

    public void Dispose()
           => _innerHost.Dispose();


    public ValueTask DisposeAsync()
        => ((IAsyncDisposable)_innerHost).DisposeAsync();


    public static WPFHostBuilder CreateBuilder()
    {
        return new WPFHostBuilder();
    }
}
