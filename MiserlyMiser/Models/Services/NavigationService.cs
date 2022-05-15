using MiserlyMiser.Infrastructure.Stores;
using MiserlyMiser.Models.Services.Interfaces;
using MiserlyMiser.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Services
{
    public class NavigationService<TViewModel> : INavigationService
        where TViewModel : ViewModel
    {
        protected NavigationStore _navigationStore;
        protected readonly Func<TViewModel> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate() => _navigationStore.CurrentViewModel = _createViewModel();
    }
}
