using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.ViewModels.Base
{
    public class ClosableViewModel : ViewModel
    {
        public event EventHandler<ClosableViewModelEventArgs> CloseWindow;
        protected void OnCloseWindow(ClosableViewModelEventArgs args) => CloseWindow?.Invoke(this, args);
    }

    public class ClosableViewModelEventArgs : EventArgs
    {
        public bool DialogResult { get; set; }
        public ClosableViewModelEventArgs(bool dialogResult) => DialogResult = dialogResult;
    }
}
