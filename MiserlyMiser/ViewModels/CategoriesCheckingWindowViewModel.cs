using MiserlyMiser.ViewModels.ViewableEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.ViewModels
{
    public class CategoriesCheckingWindowViewModel : CategoriesPageViewModel
    {
        private bool _isChecked = false;
        public bool IsChecked { get => _isChecked; set => Set(ref _isChecked, value); }

        protected override void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                SelectedItem = (ViewableCategory)sender;
            }
        }
    }
}
