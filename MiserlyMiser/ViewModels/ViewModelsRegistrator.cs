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
            .AddTransient<CashWindowViewModel>()
            .AddTransient<CashesPageViewModel>()
            .AddTransient<FinancialGoalWindowDialogViewModel>()
            .AddTransient<FinancialPageViewModel>()
            .AddTransient<TransactionWindowViewModel>()
            .AddTransient<TransactionPageViewModel>()
            .AddTransient<CategoriesPageViewModel>()
            .AddTransient<CategoriesCheckingWindowViewModel>()
            .AddTransient<SelectParentCategoryWindowViewModel>()
            .AddTransient<SelectChildrenCategoryWindowViewModel>()
        ;
    }
}
