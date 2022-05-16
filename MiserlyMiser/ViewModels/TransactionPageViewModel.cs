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
    public class TransactionPageViewModel : ViewModel
    {
        ITransactionRepository _transactionRepository;
        public TransactionPageViewModel(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
            InitCollection();
            CreateCommand = new LambdaCommand(OnCreateCommandExecuted, CanCreateCommandExecute);
            EditCommand = new LambdaCommand(OnEditCommandExecuted, CanEditCommandExecute);
            DeleteCommand = new LambdaCommand(OnDeleteCommandExecuted, CanDeleteCommandExecute);
        }
        public ObservableCollection<Transaction> Transactions { get; set; }
        private Transaction _selectedTransaction;
        public Transaction SelectedTransaction { get => _selectedTransaction; set => Set(ref _selectedTransaction, value); }

        #region Commands
        #region CreateCommand
        public ICommand CreateCommand { get; }
        private async void OnCreateCommandExecuted(object p)
        {
            IUserDialog<Transaction> userDialog = App.Services.GetRequiredService<DefaultUserDialog<TransactionWindowViewModel, TransactionWindow, Transaction>>();
            if (userDialog.Show(new Models.Dto.EntityDto<Transaction>("Новая транзакция", null, _transactionRepository)))
            {
                var transactions = await _transactionRepository.GetAllAsync();
                if (transactions != null && transactions.Count > 0)
                {
                    Transactions.Add(transactions.Last());
                }
            }
        }
        private bool CanCreateCommandExecute(object p) => true;
        #endregion

        #region EditCommand
        public ICommand EditCommand { get; }
        private void OnEditCommandExecuted(object p)
        {

        }
        private bool CanEditCommandExecute(object p) => SelectedTransaction != null;
        #endregion

        #region DeleteCommand
        public ICommand DeleteCommand { get; }
        private async void OnDeleteCommandExecuted(object p)
        {
            await _transactionRepository.DeleteAsync(SelectedTransaction);
            Transactions.Remove(SelectedTransaction);
            SelectedTransaction = null;
        }
        private bool CanDeleteCommandExecute(object p) => SelectedTransaction != null;
        #endregion
        #endregion

        private void InitCollection()
        {
            var transactions = _transactionRepository.GetAll();
            if (transactions == null || transactions.Count == 0)
                Transactions = new ObservableCollection<Transaction>();
            else
                Transactions = new ObservableCollection<Transaction>(transactions);
        }
    }
}
