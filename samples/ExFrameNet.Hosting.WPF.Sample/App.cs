using ExFrameNet.Hosting.Plugins.Abstraction;
using System;
using System.Windows;

namespace ExFrameNet.Hosting.WPF.Sample;


/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public class App : Application
{
    private readonly IServiceProvider _services;
    private readonly IPluginManager _pluginManager;

    public App(IServiceProvider serviceProvider, IPluginManager pluginManager)
    {
        _services = serviceProvider;
        _pluginManager = pluginManager;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MainWindow = new MainWindow();
        MainWindow.Show();
        _pluginManager.LoadPlugins(_services);
        
    }
}
