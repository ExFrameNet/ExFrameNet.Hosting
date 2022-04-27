using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;
using System.Windows.Threading;

namespace ExFrameNet.Hosting.WPF.Internal;
internal class HostedWPFAppService : IHostedService
{

    private readonly IServiceProvider _services;
    private readonly WPFAppContext _appContext;
    private Application? _app;


    public HostedWPFAppService(IServiceProvider services, WPFAppContext context)
    {
        _services = services;
        _appContext = context;
    }



    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }

        var thread = new Thread(WpfThread);
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_appContext.IsRunning)
        {
            _appContext.IsRunning = false;
            await _appContext.Dispatcher.InvokeAsync(() => _app?.Shutdown());
        }
    }

    private void WpfThread(object? obj)
    {
        SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));

        var app = _services.GetRequiredService<Application>();
        _app = app;
        app.Exit += (s,e) => Shutdown();
        _appContext.Dispatcher = Dispatcher.CurrentDispatcher;

        app.Resources = new ResourceDictionary();

        var resActions = _services.GetService <IEnumerable<Action<ResourceDictionary>>>();
        foreach (var action in resActions)
        {
            action(app.Resources);
        }

        _appContext.IsRunning = true;
        app.Run();
    }

    private void Shutdown()
    {
        if (!_appContext.IsRunning)
        {
            return;
        }

        _appContext.IsRunning = false;
        
        if (!_appContext.IsLifetimeLinked)
        {
            return;
        }
        
        _services.GetService<IHostApplicationLifetime>()?.StopApplication();
    }
}
