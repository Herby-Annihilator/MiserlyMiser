using Microsoft.Extensions.DependencyInjection;
using MiserlyMiser.Infrastructure.Commands;
using MiserlyMiser.Models.Dto;
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
    public class CashesPageViewModel : ViewModel
    {
        private ICrudRepository<Cash> _cashRepository;
        private ICrudRepository<Currency> _currencyRepository;
        private ICrudRepository<CashType> _cashTypesRepository;
        public CashesPageViewModel(ICrudRepository<Cash> cashRepository, ICrudRepository<Currency> currencyRepository,
            ICrudRepository<CashType> cashTypesRepository)
        {
            _cashTypesRepository = cashTypesRepository;
            _currencyRepository = currencyRepository;
            _cashRepository = cashRepository;
            InitCollections();

            AddCashCommand = new LambdaCommand(OnAddCashCommandExecuted, CanAddCashCommandExecute);
        }

        public ObservableCollection<Cash> Cashes { get; private set; }

        public ICommand AddCashCommand { get; }
        private async void OnAddCashCommandExecuted(object p)
        {
            IUserDialog<Cash> userDialog = 
                App.Services.GetRequiredService<DefaultUserDialog<CashWindowViewModel, CashWindowDialog, Cash>>();
            userDialog.Show(new EntityDto<Cash>("Новый счет", null, _cashRepository));
            var cashes = await _cashRepository.GetAllAsync();
            if (cashes != null && cashes.Count > 0)
                Cashes.Add(cashes.Last());
        }
        private bool CanAddCashCommandExecute(object p)
        {
            return true;
        }

        private void InitCollections()
        {
            IEnumerable<Cash> cashes = _cashRepository.GetAll();
            if (cashes == null)
                Cashes = new ObservableCollection<Cash>();
            else
            {
                Cashes = new ObservableCollection<Cash>(cashes);
            }
        }
    }
}
