using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Entities
{
    public class TransactionStatus : NamedEntity
    {
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
