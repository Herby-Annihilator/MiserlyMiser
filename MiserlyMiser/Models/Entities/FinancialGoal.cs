using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Entities
{
    public class FinancialGoal : NamedEntity
    {
        [MaxLength(1024)]
        public string Description { get; set; }

        [Column(TypeName = "money")]
        public decimal TargetMoneyAmount { get; set; }

        [Column(TypeName = "money")]
        public decimal CurrentMoneyAmount { get; set; }

        public Period? Period { get; set; }
        public int? PerioadId { get; set; }
    }
}
