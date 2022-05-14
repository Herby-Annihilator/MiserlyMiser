using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Entities
{
    public class Cash : NamedEntity
    {
        [Column(TypeName = "money")]
        public decimal Money { get; set; }

        public CashType CashType { get; set; }
        public int CashTypeId { get; set; }

        public Currency Currency { get; set; }
        public int CurrencyId { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }

        public ICollection<Budget> Budgets { get; set; }
    }
}
