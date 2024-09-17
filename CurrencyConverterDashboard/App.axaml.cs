using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CurrencyConverterDashboard.ViewModels;
using CurrencyConverterDashboard.Views;
using CurrencyConverterDashboard.Services;
using CurrencyConverterDashboard.Services.ApiClients;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace CurrencyConverterDashboard
{
    public class App : Application
    {
        private IServiceProvider? ServiceProvider { get; set; }

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                try
                {
                    var services = new ServiceCollection();
                    ConfigureServices(services);
                    ServiceProvider = services.BuildServiceProvider();

                    desktop.MainWindow = new MainWindow
                    {
                        DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>(),
                    };
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error during application initialization: {ex}");
                    // You might want to show an error message to the user here
                    throw; // Re-throw the exception to crash the app if it can't start properly
                }
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register your API clients
            services.AddSingleton<IApiClient, Api1Client>();
            services.AddSingleton<IApiClient, Api2Client>();

            // Register the CurrencyService
            services.AddSingleton<ICurrencyService, CurrencyService>();

            // Register the MainWindowViewModel
            services.AddSingleton<MainWindowViewModel>();
        }
    }
}