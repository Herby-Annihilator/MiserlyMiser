using MiserlyMiser.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MiserlyMiser.ViewModels
{
    public class SelectParentCategoryWindowViewModel : CategoriesPageViewModel
    {
        public SelectParentCategoryWindowViewModel() : base()
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
                Dto.Entity = SelectedItem.Category;
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
