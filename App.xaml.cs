using System.Configuration;
using System.Data;
using System.Windows;
using Hotel.Core;
using Hotel.MVVM.Viewmodel;
using Hotel.Repositories;
using Hotel.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<MainWindow>(provider => new MainWindow()
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>();
            services.AddSingleton<ReservationsViewModel>();
            services.AddSingleton<RoomsViewModel>();
            services.AddSingleton<CategoriesViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<ICategoriesRepository, CategoriesRepository>();

            services.AddSingleton<Func<Type, ViewModel>>(serviceProvider =>
                viewModelType => (ViewModel)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }

}
