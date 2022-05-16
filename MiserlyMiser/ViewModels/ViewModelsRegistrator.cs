using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiserlyMiser.ViewModels
{
    public static class ViewModelsRegistrator
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services) => services
           .AddSingleton<MainWindowViewModel>()
            .AddScoped<CashWindowViewModel>()
            .AddScoped<CashesPageViewModel>()
            .AddScoped<FinancialGoalWindowDialogViewModel>()
            .AddScoped<FinancialPageViewModel>()
            .AddScoped<TransactionWindowViewModel>()
        ;
    }
}
