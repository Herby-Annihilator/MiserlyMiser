using Microsoft.Extensions.DependencyInjection;
using MiserlyMiser.Models.Dto;
using MiserlyMiser.Models.Entities.Base;
using MiserlyMiser.Models.Services.Interfaces;
using MiserlyMiser.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MiserlyMiser.Models.Services
{
    public class DefaultUserDialog<TViewModel, TWindow, T> : IUserDialog<T>
        where T : Entity
        where TViewModel : UserDialogViewModel<T>
        where TWindow : Window, new()
    {
        public virtual bool Show(EntityDto<T> dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));
            TViewModel viewModel = App.Services.GetRequiredService<TViewModel>();
            viewModel.Dto = dto;
            TWindow window = new TWindow();
            window.DataContext = viewModel;
            window.Owner = App.Current.MainWindow;
            window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            viewModel.CloseWindow += (_, e) =>
            {
                window.DialogResult = e.DialogResult;
                window.Close();
            };
            return window.ShowDialog() ?? false;
        }
    }
}
