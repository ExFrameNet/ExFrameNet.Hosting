
using ExFrameNet.Hosting.Plugins;
using ExFrameNet.Hosting.Plugins.Sample;
using ExFrameNet.Hosting.Plugins.Sample.Plugin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder()
   .ConfigureAndLoadPlugins(p => p.Add<SamplePlugin>())
   .ConfigureServices(s => s.AddHostedService<ConsoleService>())
   .RunConsoleAsync();
