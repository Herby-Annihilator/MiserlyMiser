using MiserlyMiser.Models.Entities;
using MiserlyMiser.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.ViewModels.ViewableEntities
{
    public class ViewableCategory : ViewModel
    {
        public Category Category { get; set; }

        private ViewableCategory _parent;
        public ViewableCategory Parent { get => _parent; set => Set(ref _parent, value); }

        public ObservableCollection<ViewableCategory> Children { get; set; } = new ObservableCollection<ViewableCategory>();

        private bool _isSelected = false;
        public bool IsSelected { get => _isSelected; set => Set(ref _isSelected, value);}

        private bool _isChecked = false;
        public bool IsChecked 
        { 
            get => _isChecked;
            set
            {
                Set(ref _isChecked, value);
                Children.Traverse(c => c.Children).Each(c => c.IsChecked = _isChecked);
            }
        }
    }
}
