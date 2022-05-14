using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Entities.Base
{
    public class NamedEntity : Entity
    {
        public virtual string Name { get; set; } = "";
    }
}
