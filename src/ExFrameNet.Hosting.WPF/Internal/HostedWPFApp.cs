using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Threading;

namespace ExFrameNet.Hosting.WPF.Internal;
internal class HostedWPFApp : IHostedService
{

    private readonly IServiceProvider _services;
    private readonly WPFAppContext _appContext;
    private readonly ILogger _logger;
    private Application? _app;


    public HostedWPFApp(IServiceProvider services, WPFAppContext context, ILogger<HostedWPFApp> logger)
    {
        _services = services;
        _appContext = context;
        _logger = logger;
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
            _logger.LogInformation("Shutdown app because the host was stoped");
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

        _logger.LogInformation("Starting application");

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
        _logger.LogInformation("Shutting down host because the app was closed");
        _services.GetService<IHostApplicationLifetime>()?.StopApplication();
    }
}
