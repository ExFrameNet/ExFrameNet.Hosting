using ExFrameNet.Hosting.Plugins;
using ExFrameNet.Hosting.Plugins.Abstraction;
using ExFrameNet.Hosting.WPF.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace ExFrameNet.Hosting.WPF;
public class WPFHostBuilder
{
    private readonly IHostBuilder _hostBuilder;

    private ILoggingBuilder? _loggingBuilder;
    private readonly List<KeyValuePair<string, string>> _hostConfigurationValues;

    public IServiceCollection Services { get; } = new ServiceCollection();

    public IPluginCollection Plugins { get; } = new PluginCollection();

    public IPluginLoaderCollection PluginLoaders { get; } = new PluginLoaderCollection();

    public ILoggingBuilder Logging => _loggingBuilder ??= InitializeLogging();

    public IDictionary<object, object> Properties { get; }


    public ConfigurationManager Configuration { get; } = new ConfigurationManager();
    public HostOptions HostOptions { get; } = new HostOptions();
    public HostConfiguration HostConfiguration { get; }
    public WPFAppContext AppContext { get; } = new WPFAppContext();

    internal WPFHostBuilder()
    {
        Properties = new Dictionary<object, object>();
        _hostBuilder = Host.CreateDefaultBuilder();
        _hostConfigurationValues = new List<KeyValuePair<string, string>>(Configuration.AsEnumerable());

        var env = new HostingEnvironment()
        {
            ApplicationName = Configuration[HostDefaults.ApplicationKey],
            EnvironmentName = Configuration[HostDefaults.EnvironmentKey] ?? Environments.Production,
            ContentRootPath = HostingPathResolver.ResolvePath(Configuration[HostDefaults.ContentRootKey]),
        };

        var hostContext = new HostBuilderContext(_hostBuilder.Properties)
        {
            Configuration = Configuration,
            HostingEnvironment = env
        };

        HostConfiguration = new HostConfiguration(hostContext, Configuration, Services);

        Services.AddSingleton(AppContext);
        Services.AddHostedService<HostedWPFApp>();
    }

    public WPFHost Build()
    {

        _hostBuilder.ConfigureHostConfiguration(builder =>
        {
            builder.AddInMemoryCollection(_hostConfigurationValues);
        });

        var chainedSource = new TrackingChainedConfigurationSource(Configuration);

        _hostBuilder.ConfigureAppConfiguration(builder =>
        {
            builder.Add(chainedSource);

            foreach (var (key, value) in ((IConfigurationBuilder)Configuration).Properties)
            {
                builder.Properties[key] = value;
            }

        });

        _hostBuilder.ConfigureServices((context, services) =>
        {
            foreach (var service in Services)
            {
                services.Add(service);
            }

            var hostBuilderProviders = ((IConfigurationRoot)context.Configuration).Providers;

            if (!hostBuilderProviders.Contains(chainedSource.BuiltProvider))
            {
                ((IConfigurationBuilder)Configuration).Sources.Clear();
            }


            foreach (var provider in hostBuilderProviders)
            {
                if (!ReferenceEquals(provider, chainedSource.BuiltProvider))
                {
                    ((IConfigurationBuilder)Configuration).Add(new ConfigurationProviderSource(provider));
                }
            }
        });

        _hostBuilder.ConfigureHostOptions((c, h) =>
        {
            h.ShutdownTimeout = HostOptions.ShutdownTimeout;
            h.BackgroundServiceExceptionBehavior = HostOptions.BackgroundServiceExceptionBehavior;
        });

        HostConfiguration.RunDeferredCallbacks(_hostBuilder);

        _hostBuilder.ConfigurePluginLoaders(l =>
        {
            l.AddFromCollection(PluginLoaders);
        });

        _hostBuilder.ConfigureAndLoadPlugins(p =>
        {
            foreach (var plugin in Plugins)
            {
                p.Add(plugin);
            }
        });

        var host = new WPFHost(_hostBuilder.Build());

        host.Services.GetService<IEnumerable<IConfiguration>>();
        return host;
    }

    public void ConfigureResources(Action<ResourceDictionary> builder)
    {
        Services.AddSingleton(builder);
    }

    private ILoggingBuilder InitializeLogging()
    {
        return new LoggingBuilder(Services);
    }

    private sealed class LoggingBuilder : ILoggingBuilder
    {
        public IServiceCollection Services { get; }

        public LoggingBuilder(IServiceCollection services)
        {
            Services = services;
        }
    }
}
