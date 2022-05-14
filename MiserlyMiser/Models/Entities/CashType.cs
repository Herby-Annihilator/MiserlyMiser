using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Entities
{
    public class CashType : NamedEntity
    {
        public ICollection<Cash>? Cashes { get; set; }
    }
}
