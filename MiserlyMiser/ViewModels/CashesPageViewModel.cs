using MiserlyMiser.Models.Entities;
using MiserlyMiser.Models.Repositories.Interfaces;
using MiserlyMiser.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.ViewModels
{
    public class CashesPageViewModel : ViewModel
    {
        private ICrudRepository<Cash> _cashRepository;
        public CashesPageViewModel(ICrudRepository<Cash> cashRepository)
        {
            _cashRepository = cashRepository;
            Cashes = new ObservableCollection<Cash>(_cashRepository.GetAll());
        }

        public ObservableCollection<Cash> Cashes { get; }
    }
}
