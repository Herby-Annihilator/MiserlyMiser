using MiserlyMiser.Infrastructure.Commands;
using MiserlyMiser.Models.Entities;
using MiserlyMiser.Models.Repositories.Interfaces;
using MiserlyMiser.ViewModels.ViewableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiserlyMiser.ViewModels
{
    public class SelectChildrenCategoryWindowViewModel : CategoriesCheckingWindowViewModel
    {
        public SelectChildrenCategoryWindowViewModel(ICategoryRepository categoryRepository) : base(categoryRepository)
        {
            SelectCategoryCommand = new LambdaCommand(OnSelectCategoryCommandExecuted, CanSelectCategoryCommandExecute);
            CancelCommand = new LambdaCommand(OnCancelCommandExecuted, CanCancelCommandExecute);
        }

        public ICommand SelectCategoryCommand { get; }
        private void OnSelectCategoryCommandExecuted(object p)
        {
            try
            {
                Status = "";
                IEnumerable<ViewableCategory> categories = ViewableCategories.Traverse(item => item.Children)
                    .Where(item => item.IsChecked);
                List<Category> children = new List<Category>();
                foreach (var category in categories)
                {
                    children.Add(category.Category);
                }
                Dto.Entity.ChildCategories = children;
                OnCloseWindow(new Base.ClosableViewModelEventArgs(true));
            }
            catch (Exception ex)
            {
                Status = ex.Message;
            }
        }
        private bool CanSelectCategoryCommandExecute(object p) => SelectedItem != null;

        public ICommand CancelCommand { get; }
        private void OnCancelCommandExecuted(object p)
        {
            OnCloseWindow(new Base.ClosableViewModelEventArgs(false));
        }
        private bool CanCancelCommandExecute(object p) => true;
    }
}
