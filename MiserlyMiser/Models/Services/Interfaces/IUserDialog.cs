using MiserlyMiser.Models.Dto;
using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Services.Interfaces
{
    public interface IUserDialog<T> where T : Entity
    {
        bool Show(EntityDto<T> dto);
    }
}
