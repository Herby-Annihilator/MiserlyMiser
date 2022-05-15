using MiserlyMiser.Infrastructure.Commands;
using MiserlyMiser.Models.Entities;
using MiserlyMiser.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiserlyMiser.ViewModels
{
    public class FinancialGoalWindowDialogViewModel : UserDialogViewModel<FinancialGoal>
    {
        public FinancialGoalWindowDialogViewModel()
        {
            CancelCommand = new LambdaCommand(OnCancelCommandExecuted, CanCancelCommandExecute);
            ApplyCommand = new LambdaCommand(OnApplyCommandExecuted, CanApplyCommandExecute);
        }
        private string _name = "";
        public string Name { get => _name; set => Set(ref _name, value); }

        private string _desiredAmountOfMoney = "";
        public string DesiredAmountOfMoney { get => _desiredAmountOfMoney; set => Set(ref _desiredAmountOfMoney, value); }

        private string _currentAmountOfMoney = "";
        public string CurrentAmountOfMoney { get => _currentAmountOfMoney; set => Set(ref _currentAmountOfMoney, value); }

        private string _description = "";
        public string Description { get => _description; set => Set(ref _description, value); }

        private DateTime _desiredDate = DateTime.Now;
        public DateTime DesiredDate { get => _desiredDate; set => Set(ref _desiredDate, value); }

        public override void SetProperties()
        {
            if (Dto == null)
                return;
            Name = Dto.Entity?.Name;
            Description = Dto.Entity?.Description;
            CurrentAmountOfMoney = Dto.Entity?.CurrentMoneyAmount.ToString();
            DesiredAmountOfMoney = Dto.Entity?.TargetMoneyAmount.ToString();
        }

        public ICommand CancelCommand { get; }
        private void OnCancelCommandExecuted(object p)
        {
            OnCloseWindow(new ClosableViewModelEventArgs(false));
        }
        private bool CanCancelCommandExecute(object p) => true;

        public ICommand ApplyCommand { get; }
        private void OnApplyCommandExecuted(object p)
        {
            try
            {
                Status = "";
                FinancialGoal financialGoal;
                if (_isCreate)
                    financialGoal = new FinancialGoal();
                else
                    financialGoal = Dto.Entity;
                financialGoal.Name = Name;
                financialGoal.Description = Description;
                financialGoal.Period = new Period() { EndDate = DesiredDate, StartDate = DateTime.Now };
                financialGoal.Period.FinancialGoals = new List<FinancialGoal>() { financialGoal };
                financialGoal.CurrentMoneyAmount = Convert.ToDecimal(CurrentAmountOfMoney.Replace('.', ','));
                financialGoal.TargetMoneyAmount = Convert.ToDecimal(DesiredAmountOfMoney.Replace('.', ','));
                if (_isCreate)
                    Dto.Repository.Create(financialGoal);
                else
                    Dto.Repository.Update(financialGoal);
                OnCloseWindow(new ClosableViewModelEventArgs(true));
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }           
        }
        private bool CanApplyCommandExecute(object p) => !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(Description)
            && !string.IsNullOrWhiteSpace(CurrentAmountOfMoney)
            && !string.IsNullOrWhiteSpace(DesiredAmountOfMoney)
            && IsDateValid();
        private bool IsDateValid() => DesiredDate > DateTime.Now;
    }
}
