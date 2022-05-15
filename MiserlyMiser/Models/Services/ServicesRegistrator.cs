using Microsoft.Extensions.DependencyInjection;
using MiserlyMiser.Models.Entities;
using MiserlyMiser.Models.Services.Interfaces;
using MiserlyMiser.ViewModels;
using MiserlyMiser.Views.Windows;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiserlyMiser.Models.Services
{
    public static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddScoped<DefaultUserDialog<CashWindowViewModel, CashWindowDialog, Cash>>()
            .AddScoped<DefaultUserDialog<FinancialGoalWindowDialogViewModel, FinancialGoalWindowDialog, FinancialGoal>>()
        // Register your services here
        ;
    }
}
