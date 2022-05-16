using Microsoft.Extensions.DependencyInjection;
using MiserlyMiser.Infrastructure.Commands;
using MiserlyMiser.Models.Services.Interfaces;
using MiserlyMiser.ViewModels.Base;
using MiserlyMiser.Views.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Markup;

namespace MiserlyMiser.ViewModels
{
    [MarkupExtensionReturnType(typeof(MainWindowViewModel))]
    public class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            ChangeViewCommand = new LambdaCommand(OnChangeViewCommandExecuted, CanChangeViewCommandExecute);
        }
        private ViewModel _currentViewModel;
        public ViewModel CurrentViewModel { get => _currentViewModel; set => Set(ref _currentViewModel, value); }

        #region Properties
        private string _title = "Title";
        public string Title { get => _title; set => Set(ref _title, value); }

        private string _status = "Status";
        public string Status { get => _status; set => Set(ref _status, value); }

        public string CashViewName => nameof(CashesPage);
        public string FinancialGoalsViewName => nameof(FinancialGoalPage);
        public string TransactionsViewName => nameof(TransactionPage);
        #endregion

        #region Commands
        public ICommand ChangeViewCommand { get; }
        private void OnChangeViewCommandExecuted(object p)
        {
            try
            {
                Status = "";
                if (p != null && p is string name)
                {
                    if (name == CashViewName)
                    {
                        CurrentViewModel = App.Services.GetRequiredService<CashesPageViewModel>();
                    }
                    else if (name == FinancialGoalsViewName)
                    {
                        CurrentViewModel = App.Services.GetRequiredService<FinancialPageViewModel>();
                    }
                    else if (name == TransactionsViewName)
                    {
                        CurrentViewModel = App.Services.GetRequiredService<TransactionPageViewModel>();
                    }
                    Status = "Переключение представления";
                }               
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
            
        }
        private bool CanChangeViewCommandExecute(object p) => true;
        #endregion
    }
}
