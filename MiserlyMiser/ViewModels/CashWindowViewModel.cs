using MiserlyMiser.Infrastructure.Commands;
using MiserlyMiser.Models.Entities;
using MiserlyMiser.Models.Repositories.Interfaces;
using MiserlyMiser.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiserlyMiser.ViewModels
{
    public class CashWindowViewModel : UserDialogViewModel<Cash>
    {
        private ICrudRepository<Currency> _currencyRepository;
        private ICrudRepository<CashType> _cashTypeRepository;
        public CashWindowViewModel(ICrudRepository<Currency> currencyRepository, ICrudRepository<CashType> cashTypeRepository)
        {
            _currencyRepository = currencyRepository;
            _cashTypeRepository = cashTypeRepository;

            CashTypes = new ObservableCollection<CashType>(_cashTypeRepository.GetAll());
            Currencies = new ObservableCollection<Currency>(_currencyRepository.GetAll());

            ApplyCommand = new LambdaCommand(OnApplyCommandExecuted, CanApplyCommandExecute);
            CancelCommand = new LambdaCommand(OnCancelCommandExecuted, CanCancelCommandExecute);
        }

        #region Properties

        private string _name = "";
        public string Name { get => _name; set => Set(ref _name, value); }

        private string _description = "";
        public string Description { get => _description; set => Set(ref _description, value); }

        private string _money = "";
        public string Money { get => _money; set => Set(ref _money, value); }

        public ObservableCollection<CashType> CashTypes { get; private set; }
        private CashType _selectedCashType;
        public CashType SelectedCashType { get => _selectedCashType; set => Set(ref _selectedCashType, value); }

        public ObservableCollection<Currency> Currencies { get; private set; }
        private Currency _selectedCurrency;
        public Currency SelectedCurrency { get => _selectedCurrency; set => Set(ref _selectedCurrency, value); }

        #endregion

        #region Commands

        #region ApplyCommand
        public ICommand ApplyCommand { get; }
        private void OnApplyCommandExecuted(object p)
        {
            try
            {
                Status = "";
                Cash cash;
                if (_isCreate)
                    cash = new Cash();
                else
                    cash = Dto.Entity;
                cash.Name = Name;
                cash.Currency = SelectedCurrency;
                cash.CurrencyId = SelectedCurrency.Id;
                cash.Money = Convert.ToDecimal(Money.Replace('.', ','));
                cash.CashType = SelectedCashType;
                cash.CashTypeId = SelectedCashType.Id;
                if (_isCreate)
                    Dto.Repository.Create(cash);
                else
                    Dto.Repository.Update(cash);
                OnCloseWindow(new ClosableViewModelEventArgs(true));
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }          
        }
        private bool CanApplyCommandExecute(object p) => true;
        #endregion

        #region CancelCommand
        public ICommand CancelCommand { get; }
        private void OnCancelCommandExecuted(object p)
        {
            OnCloseWindow(new ClosableViewModelEventArgs(false));
        }
        private bool CanCancelCommandExecute(object p) => true;
        #endregion

        #endregion


        public override void SetProperties()
        {
            if (Dto == null)
                return;
            Name = Dto.Entity?.Name;
            Description = "";
            Money = Dto.Entity?.Money.ToString();
        }
    }
}
