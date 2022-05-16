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
    public class TransactionWindowViewModel : UserDialogViewModel<Transaction>
    {
        private ICrudRepository<Cash> _cashRepository;
        private ICrudRepository<PaymentType> _paymentRepository;
        private ICrudRepository<TransactionStatus> _transactionStatusRepository;
        public TransactionWindowViewModel(ICrudRepository<Cash> cashRepo, ICrudRepository<PaymentType> paymentRepo, ICrudRepository<TransactionStatus> transactionStatusRepo)
        {
            _cashRepository = cashRepo;
            _paymentRepository = paymentRepo;
            _transactionStatusRepository = transactionStatusRepo;

            InitCollections();

            CancelCommand = new LambdaCommand(OnCancelCommandExecuted, CanCancelCommandExecute);
            ApplyCommand = new LambdaCommand(OnApplyCommandExecuted, CanApplyCommandExecute);
            SelectRadioButtonCommand = new LambdaCommand(OnSelectRadioButtonCommandExecuted,
                CanSelectRadioButtonCommandExecute);
            SelectCategoryCommand = new LambdaCommand(OnSelectCategoryCommandExecuted, CanSelectCategoryCommandExecute);
        }

        #region Properties
        private string _name = "";
        public string Name { get => _name; set => Set(ref _name, value); }

        private string _money = "";
        public string Money { get => _money; set => Set(ref _money, value); }

        private bool _isIncome = true;
        public bool IsIncome { get => _isIncome; set => Set(ref _isIncome, value); }

        private bool _isSpending = false;
        public bool IsSpending { get => _isSpending; set => Set(ref _isSpending, value); }

        private bool _isTransfer = false;
        public bool IsTransfer { get => _isTransfer; set => Set(ref _isTransfer, value); }

        public string Income { get; } = nameof(Income);
        public string Spending { get; } = nameof(Spending);
        public string Transfer { get; } = nameof(Transfer);

        public ObservableCollection<Cash> Cashes { get; private set; }
        private Cash _selectedCash;
        public Cash SelectedCash
        {
            get => _selectedCash;
            set
            {
                Set(ref _selectedCash, value);
                SelectedCurrency = _selectedCash?.Currency;
            }
        }

        private Cash _oldCash;
        private TransactionType _oldTransactionType;
        private decimal _oldMoney;

        // зависит от выбранного счета
        private Currency _selectedCurrency;
        public Currency SelectedCurrency { get => _selectedCurrency; set => Set(ref _selectedCurrency, value); }

        public ObservableCollection<PaymentType> PaymentTypes { get; private set; }
        private PaymentType _selectedPaymentType;
        public PaymentType SelectedPaymentType { get => _selectedPaymentType; set => Set(ref _selectedPaymentType, value); }

        public ObservableCollection<TransactionStatus> TransactionStatuses { get; private set; }
        private TransactionStatus _selectedTransactionStatus;
        public TransactionStatus SelectedTransactionStatus
        {
            get => _selectedTransactionStatus;
            set => Set(ref _selectedTransactionStatus, value);
        }

        private Category _selectedCategory;
        public Category SelectedCategory { get => _selectedCategory; set => Set(ref _selectedCategory, value); }
        #endregion

        #region Commands
        #region CancelCommand
        public ICommand CancelCommand { get; }
        private void OnCancelCommandExecuted(object p)
        {
            OnCloseWindow(new ClosableViewModelEventArgs(false));
        }
        private bool CanCancelCommandExecute(object p) => true;
        #endregion

        #region ApplyCommand
        public ICommand ApplyCommand { get; }
        private void OnApplyCommandExecuted(object p)
        {
            try
            {
                Status = "";
                Transaction transaction;
                if (_isCreate)
                    transaction = new Transaction();
                else
                    transaction = Dto.Entity;

                transaction.Name = Name;
                transaction.Money = Convert.ToDecimal(Money.Replace('.', ','));
                transaction.TransactionStatus = SelectedTransactionStatus;
                transaction.TransactionType = IsIncome ? TransactionType.Incoming : TransactionType.Spending;
                if (!_isCreate)
                {
                    if (_oldCash == SelectedCash)
                    {
                        if (_oldTransactionType == TransactionType.Incoming)
                        {
                            if (IsIncome)
                            {
                                decimal toAdd = Convert.ToDecimal(Money.Replace('.', ',')) - _oldMoney;
                                _oldCash.Money += toAdd;
                            }
                            else // spending
                            {
                                _oldCash.Money += -Convert.ToDecimal(Money.Replace('.', ',')) - _oldMoney;
                            }
                        }
                        else // Spending
                        {
                            if (IsIncome)
                            {
                                _oldCash.Money += _oldMoney + Convert.ToDecimal(Money.Replace('.', ','));
                            }
                            else  // spending
                            {
                                _oldCash.Money += _oldMoney - Convert.ToDecimal(Money.Replace('.', ','));
                            }
                        }
                    }
                    else // разные счета
                    {
                        if (_oldTransactionType == TransactionType.Incoming)
                        {
                            if (IsIncome)
                            {
                                _oldCash.Money -= _oldMoney;
                                SelectedCash.Money += Convert.ToDecimal(Money.Replace('.', ','));
                            }
                            else
                            {
                                _oldCash.Money += _oldMoney;
                                SelectedCash.Money -= Convert.ToDecimal(Money.Replace('.', ','));
                            }
                        }
                        else // Spending
                        {
                            if (IsIncome)
                            {
                                _oldCash.Money += _oldMoney;
                                SelectedCash.Money += Convert.ToDecimal(Money.Replace('.', ','));
                            }
                            else
                            {
                                _oldCash.Money += _oldMoney;
                                SelectedCash.Money -= Convert.ToDecimal(Money.Replace('.', ','));
                            }
                        }                       
                    }
                    SelectedCash.Transactions.Remove(SelectedCash.Transactions.FirstOrDefault(t => t.Id == transaction.Id));
                    SelectedCash.Transactions.Add(transaction);
                    _cashRepository.Update(_oldCash);
                    _cashRepository.Update(SelectedCash);
                }
                else
                {
                    if (IsIncome)
                    {
                        SelectedCash.Money += Convert.ToDecimal(Money.Replace('.', ','));
                    }
                    else
                    {
                        SelectedCash.Money -= Convert.ToDecimal(Money.Replace('.', ','));
                    }
                    if (SelectedCash.Transactions == null)
                        SelectedCash.Transactions = new List<Transaction>() { transaction };
                    else
                        SelectedCash.Transactions.Add(transaction);
                    _cashRepository.Update(SelectedCash);
                }

                if (_isCreate)
                    Dto.Repository.Create(transaction);
                else
                    Dto.Repository.Update(transaction);

                OnCloseWindow(new ClosableViewModelEventArgs(true));
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }
        private bool CanApplyCommandExecute(object p) => !string.IsNullOrWhiteSpace(Name)
            && !string.IsNullOrWhiteSpace(Money)
            && SelectedCash != null
            && SelectedCategory != null
            && SelectedCurrency != null
            && SelectedPaymentType != null
            && SelectedTransactionStatus != null;
        #endregion

        #region SelectRadioButtonCommand
        public ICommand SelectRadioButtonCommand { get; }
        private void OnSelectRadioButtonCommandExecuted(object p)
        {
            try
            {
                Status = "";
                if (p != null && p is string name)
                {
                    if (name == Transfer)
                    {
                        IsTransfer = true;
                        IsSpending = false;
                        IsTransfer = false;
                    }
                    else if (name == Spending)
                    {
                        IsSpending = true;
                        IsTransfer = false;
                        IsTransfer = false;
                    }
                    else if (name == Income)
                    {
                        IsIncome = true;
                        IsSpending = false;
                        IsTransfer = false;
                    }
                }
            }
            catch (Exception e)
            {
                Status = e.Message;
            }
        }
        private bool CanSelectRadioButtonCommandExecute(object p) => true;
        #endregion

        #region SelectCategoryCommand
        public ICommand SelectCategoryCommand { get; }
        private void OnSelectCategoryCommandExecuted(object p)
        {
            OnCloseWindow(new ClosableViewModelEventArgs(false));
        }
        private bool CanSelectCategoryCommandExecute(object p) => true; 
        #endregion
        #endregion

        public override void SetProperties()
        {
            if (Dto == null || Dto.Entity == null)
                return;
            Name = Dto.Entity.Name;
            Money = Dto.Entity.Money.ToString();
            _oldMoney = Dto.Entity.Money;
            _oldTransactionType = Dto.Entity.TransactionType;
            if (Dto.Entity.TransactionType == TransactionType.Incoming)
            {
                IsIncome = true;
                IsTransfer = false;
                IsSpending = false;
            }                
            else if (Dto.Entity.TransactionType == TransactionType.Transfer)
            {
                IsTransfer = true;
                IsSpending = false;
                IsIncome = false;
            }                
            else if (Dto.Entity.TransactionType == TransactionType.Spending)
            {
                IsSpending = true;
                IsIncome = false;
                IsTransfer = false;
            }
            _oldCash = Dto.Entity.Cash;
            SelectedCash = Dto.Entity.Cash;
            SelectedCategory = Dto.Entity.Category;
            SelectedTransactionStatus = Dto.Entity.TransactionStatus;
            SelectedPaymentType = Dto.Entity.PaymentType;
        }

        public void InitCollections()
        {
            Cashes = new ObservableCollection<Cash>(_cashRepository.GetAll());
            PaymentTypes = new ObservableCollection<PaymentType>(_paymentRepository.GetAll());
            TransactionStatuses = new ObservableCollection<TransactionStatus>(_transactionStatusRepository.GetAll());
        }
    }
}
