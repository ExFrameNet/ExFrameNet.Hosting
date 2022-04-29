using ExFrameNet.Hosting.WPF;
using ExFrameNet.Hosting.WPF.Sample;

var builder = WPFHost.CreateBuilder();

builder.AddResources("Resources/Styles.xaml");
builder.WithApp<App>();

await builder.Build().StartAsync();