using MiserlyMiser.Models.Dto;
using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.ViewModels.Base
{
    public class UserDialogViewModel<T> : ClosableViewModel where T : Entity
    {
        protected EntityDto<T> _dto;
        public virtual EntityDto<T> Dto { get => _dto; set => _dto = value; }

        protected string _status = "";
        public virtual string Status { get => _status; set => Set(ref _status, value); }

        protected string _semanticString = "";
        public virtual string SemanticString { get => _semanticString; set => Set(ref _semanticString, value); }

        protected bool _isCreate = false;
    }
}
