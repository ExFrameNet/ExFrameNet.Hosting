using ExFrameNet.Hosting.Plugins.Sample.Plugin;
using ExFrameNet.Hosting.WPF;
using ExFrameNet.Hosting.WPF.Sample;

var builder = WPFHost.CreateBuilder();

builder.AddResources("Resources/Styles.xaml");
builder.WithApp<App>();
builder.Plugins.Add<SamplePlugin>();

await builder.Build().StartAsync();