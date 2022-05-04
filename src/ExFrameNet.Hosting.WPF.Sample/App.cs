using System;
using System.Windows;

namespace ExFrameNet.Hosting.WPF.Sample;


/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public class App : Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider serviceProvider)
    {
        _services = serviceProvider;
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MainWindow = new MainWindow();
        MainWindow.Show();
    }
}
