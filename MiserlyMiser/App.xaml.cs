using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiserlyMiser.Models.DataContexts;
using MiserlyMiser.Models.Entities;
using MiserlyMiser.Models.Repositories;
using MiserlyMiser.Models.Repositories.Interfaces;
using MiserlyMiser.Models.Services;
using MiserlyMiser.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MiserlyMiser
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static Window FocusedWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsFocused);

        public static Window ActivedWindow => Current.Windows.Cast<Window>().FirstOrDefault(w => w.IsActive);

        private static IHost _host;

        public static IHost Host => _host ??= Program.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

        public static IServiceProvider Services => Host.Services;

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services) => services
           .AddDbContext<MiserlyMiserDataContext>()
            .AddScoped(typeof(ICrudRepository<>), typeof(DefaultCrudRepository<>))
            .AddScoped<ICrudRepository<Cash>, CashRepository>()
            .AddScoped<ICrudRepository<Transaction>, TransactionRepository>()
            .AddScoped<ITransactionRepository, TransactionRepository>()
            .AddScoped<ICrudRepository<FinancialGoal>, FinancialGoalRepository>()
            .AddTransient<ICategoryRepository, CategoryRepository>()
            .AddServices()
           .AddViewModels()
        ;

        protected override async void OnStartup(StartupEventArgs e)
        {
            
            var host = Host;

            base.OnStartup(e);

            await host.StartAsync();

        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            using (Host) await Host.StopAsync();
        }
    }
}
