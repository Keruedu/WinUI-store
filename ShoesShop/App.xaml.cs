using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;

using ShoesShop.Activation;
using ShoesShop.Contracts.Services;
using ShoesShop.Controls;
using ShoesShop.Core.Contracts.Repository;
using ShoesShop.Core.Contracts.Services;
using ShoesShop.Core.Http;
using ShoesShop.Core.Repository;
using ShoesShop.Core.Services;
using ShoesShop.Core.Services.DataAcess;
using ShoesShop.Helpers;
using ShoesShop.Models;
using ShoesShop.Services;
using ShoesShop.ViewModels;
using ShoesShop.Views;

namespace ShoesShop;

// To learn more about WinUI 3, see https://docs.microsoft.com/windows/apps/winui/winui3/.
public partial class App : Application
{
    // The .NET Generic Host provides dependency injection, configuration, logging, and other services.
    // https://docs.microsoft.com/dotnet/core/extensions/generic-host
    // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
    // https://docs.microsoft.com/dotnet/core/extensions/configuration
    // https://docs.microsoft.com/dotnet/core/extensions/logging
    public IHost Host
    {
        get;
    }

    public static T GetService<T>()
        where T : class
    {
        if ((App.Current as App)!.Host.Services.GetService(typeof(T)) is not T service)
        {
            throw new ArgumentException($"{typeof(T)} needs to be registered in ConfigureServices within App.xaml.cs.");
        }

        return service;
    }

    public static WindowEx MainWindow { get; } = new MainWindow();

    public static UIElement? AppTitlebar { get; set; }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.
        CreateDefaultBuilder().
        UseContentRoot(AppContext.BaseDirectory).
        ConfigureServices((context, services) =>
        {
            // Default Activation Handler
            services.AddTransient<ActivationHandler<LaunchActivatedEventArgs>, DefaultActivationHandler>();

            // Other Activation Handlers

            // Http clients
            services.AddHttpClient("Backend", client =>
            {
                var host = App.GetService<IStoreServerOriginService>().Host;
                var port = App.GetService<IStoreServerOriginService>().Port;

                client.BaseAddress = new Uri(@$"{host}:{port}/api/v1/");
            }).AddHttpMessageHandler<AccessTokenHandler>().AddHttpMessageHandler<AuthenticationResponseHandler>();



            // Services
            services.AddSingleton<ICloudinaryService, CloudinaryService>();
            services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            services.AddSingleton<ILocalSettingsService, LocalSettingsService>();
            services.AddTransient<INavigationViewService, NavigationViewService>();
            services.AddTransient<IStorePageSettingsService, StorePageSettingsService>();
            services.AddTransient<IStoreLastOpenPageService, StoreLastOpenPageService>();
            services.AddSingleton<IActivationService, ActivationService>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IPageService, PageService>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddTransient<IReviewDataService, ReviewDataService>();
            services.AddTransient<IUserDataService, UserDataService>();


            // Core Services
            services.AddSingleton<IDao, PostgreDao>();
            services.AddSingleton<ISampleDataService, SampleDataService>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ICategoryDataService, CategoryDataService>();
            services.AddTransient<IShoesDataService, ShoesDataService>();
            services.AddTransient<IOrderDataService, OrderDataService>();
            services.AddSingleton<IStatisticDataService, StatisticDataService>();
            services.AddSingleton<IStatisticRepository, StatisticRepository>();
            services.AddSingleton<IStoreServerOriginService, StoreServerOriginService>();
            


            // Views and ViewModels
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<SettingsPage>();
            services.AddTransient<ImportDataViewModel>();
            services.AddTransient<ImportDataPage>();
            services.AddTransient<AddOrderViewModel>();
            services.AddTransient<AddOrderPage>();
            services.AddTransient<OrdersViewModel>();
            services.AddTransient<OrdersPage>();
            services.AddTransient<OrderDetailControlViewModel>();
            services.AddTransient<AddShoesViewModel>();
            services.AddTransient<AddShoesPage>();
            services.AddTransient<ShoesViewModel>();
            services.AddTransient<ShoesPage>();
            services.AddTransient<ShoesDetailViewModel>();
            services.AddTransient<ShoesDetailPage>();
            services.AddTransient<UsersPage>();
            services.AddTransient<UsersViewModel>();
            services.AddTransient<UserDetailPage>();
            services.AddTransient<UserDetailViewModel>();
            services.AddTransient<AddUserPage>();
            services.AddTransient<AddUserViewModel>();
            services.AddTransient<AddCategoryViewModel>();
            services.AddTransient<AddCategoryPage>();
            services.AddTransient<CategoriesViewModel>();
            services.AddTransient<CategoriesPage>();
            services.AddTransient<CategoryDetailControlViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<DashboardPage>();
            services.AddTransient<ShellPage>();
            services.AddTransient<ShellViewModel>();

            // Configuration
            services.Configure<LocalSettingsOptions>(context.Configuration.GetSection(nameof(LocalSettingsOptions)));
        }).
        Build();

        UnhandledException += App_UnhandledException;
    }
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IStatisticDataService, StatisticDataService>();
        // Các đăng ký dịch vụ khác
    }


    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        // TODO: Log and handle exceptions as appropriate.
        // https://docs.microsoft.com/windows/windows-app-sdk/api/winrt/microsoft.ui.xaml.application.unhandledexception.
    }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        base.OnLaunched(args);
        await App.GetService<IActivationService>().ActivateAsync(args);
    }
}
