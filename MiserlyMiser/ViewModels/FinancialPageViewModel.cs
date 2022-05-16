using Microsoft.Extensions.DependencyInjection;
using MiserlyMiser.Infrastructure.Commands;
using MiserlyMiser.Models.Entities;
using MiserlyMiser.Models.Repositories.Interfaces;
using MiserlyMiser.Models.Services;
using MiserlyMiser.Models.Services.Interfaces;
using MiserlyMiser.ViewModels.Base;
using MiserlyMiser.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiserlyMiser.ViewModels
{
    public class FinancialPageViewModel : ViewModel
    {
        private ICrudRepository<FinancialGoal> _repository;

        public FinancialPageViewModel(ICrudRepository<FinancialGoal> repository)
        {
            _repository = repository;
            var goals = _repository.GetAll();
            if (goals != null && goals.Count > 0)
                FinancialGoals = new ObservableCollection<FinancialGoal>(goals);
            else
                FinancialGoals = new ObservableCollection<FinancialGoal>();

            AddGoalCommand = new LambdaCommand(OnAddGoalCommandExecuted, CanAddGoalCommandExecute);
        }

        public ObservableCollection<FinancialGoal> FinancialGoals { get; }

        public ICommand AddGoalCommand { get; }
        private async void OnAddGoalCommandExecuted(object p)
        {
            IUserDialog<FinancialGoal> userDialog = App.Services.GetRequiredService<DefaultUserDialog<FinancialGoalWindowDialogViewModel, FinancialGoalWindowDialog, FinancialGoal>>();
            if (userDialog.Show(new Models.Dto.EntityDto<FinancialGoal>("Новая цель", null, _repository)))
            {
                var goals = await _repository.GetAllAsync();
                if (goals != null && goals.Count > 0)
                    FinancialGoals.Add(goals.Last());
            }
        }
        private bool CanAddGoalCommandExecute(object p) => true;

    }
}
