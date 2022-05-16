using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Entities
{
    public class Transaction : NamedEntity
    {
        public DateTime Date { get; set; } = DateTime.Now;

        [Column(TypeName = "money")]
        public decimal Money { get; set; }
        public TransactionType TransactionType { get; set; }

        public TransactionStatus TransactionStatus { get; set; }
        public int TransactionStatusId { get; set; }

        public PaymentType PaymentType { get; set; }
        public int PaymentTypeId { get; set; }

        public Cash Cash { get; set; }
        public int CashId { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
