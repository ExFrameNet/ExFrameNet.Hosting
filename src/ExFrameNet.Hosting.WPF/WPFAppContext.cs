using System.Windows.Threading;
using System.Windows;

namespace ExFrameNet.Hosting.WPF;
public class WPFAppContext
{
    public ShutdownMode ShutdownMode { get; set; } = ShutdownMode.OnMainWindowClose;
    public Dispatcher Dispatcher { get; internal set; }


    public bool IsLifetimeLinked { get; set; } = true;
    public bool IsRunning { get; internal set; }

    internal WPFAppContext()
    {
        Dispatcher = Dispatcher.CurrentDispatcher;
    }
}
