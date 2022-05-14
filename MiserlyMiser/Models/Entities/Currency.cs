using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Entities
{
    public class Currency : Entity
    {
        public string FullName { get; set; } = "";
        public string ShortName { get; set; } = "";
        public ICollection<Cash>? Cashes { get; set; }
    }
}
