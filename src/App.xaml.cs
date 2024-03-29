﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using System.Windows.Threading;

namespace PsychonautsStudio;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    // Custom entry point
    [STAThread]
    public static void Main(string[] args)
    {
        // Build the services
        IServiceProvider serviceProvider = BuildServices();

        // Create and run the app
        App app = new(serviceProvider);
        app.InitializeComponent();
        app.Run(serviceProvider.GetRequiredService<MainWindow>());
    }

    private static IServiceProvider BuildServices()
    {
        IServiceCollection services = new ServiceCollection();

        // Add app user data
        services.AddSingleton<AppUserData>();

        // Add services
        services.AddTransient<AppUIManager>();

        // Add view models
        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<OpenFileViewModel>();

        // Add windows
        services.AddSingleton<MainWindow>();
        services.AddTransient<OpenFileWindow>();

        return services.BuildServiceProvider();
    }

    public App(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;

        DispatcherUnhandledException += App_DispatcherUnhandledException;
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        ServiceProvider.GetRequiredService<AppUIManager>().ShowErrorMessage("The application crashed due to a fatal error", e.Exception);
    }

    public IServiceProvider ServiceProvider { get; }

    public new static App Current => (App)Application.Current;

    public static Version Version => new(0, 1, 2, 0);
}