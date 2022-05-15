using MiserlyMiser.Infrastructure.Commands.Base;
using MiserlyMiser.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Infrastructure.Commands
{
    public class NavigateCommand : Command
    {
        private INavigationService _navigationService;

        public NavigateCommand(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        protected override void Execute(object parameter)
        {
            _navigationService.Navigate();
        }
    }
}
