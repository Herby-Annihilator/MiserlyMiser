using MiserlyMiser.Models.Repositories.Interfaces;
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
        public CategoriesCheckingWindowViewModel(ICategoryRepository categoryRepository) : base(categoryRepository)
        {

        }
        private bool _isChecked = false;
        public bool IsChecked { get => _isChecked; set => Set(ref _isChecked, value); }

        protected override void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsSelected")
            {
                if (sender is ViewableCategory category)
                {
                    SelectedItem = category;
                    category.IsChecked = true;
                    category.Children.Traverse(c => c.Children).Each(c => c.IsChecked = true);
                }
            }           
        }
    }
}
