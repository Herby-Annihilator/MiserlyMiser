using MiserlyMiser.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Infrastructure.Stores
{
    public class NavigationStore
    {
        private ViewModel _currentViewModel;
        public ViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnViewModelChanged();
            }
        }
        public event Action ViewModelChanged;
        protected virtual void OnViewModelChanged() => ViewModelChanged?.Invoke();
    }
}
