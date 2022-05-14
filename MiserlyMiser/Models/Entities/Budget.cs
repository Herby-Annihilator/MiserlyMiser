using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Entities
{
    public class Budget : NamedEntity
    {
        [Column(TypeName = "money")]
        public decimal Money { get; set; }
        public ICollection<Cash> Cashes { get; set; }
        public ICollection<Category> Categories { get; set; }
        public Period Period { get; set; }
        public int PeriodId { get; set; }
    }
}
