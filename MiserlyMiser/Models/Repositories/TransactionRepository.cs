using Microsoft.EntityFrameworkCore;
using MiserlyMiser.Models.DataContexts;
using MiserlyMiser.Models.Entities;
using MiserlyMiser.Models.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Repositories
{
    public class TransactionRepository : DefaultCrudRepository<Transaction>, ITransactionRepository
    {
        public TransactionRepository(MiserlyMiserDataContext context) : base(context)
        {
        }

        public override ICollection<Transaction> GetAll()
        {
            return EntitySet.Include(t => t.Cash)
                .Include(t => t.Category)
                .Include(t => t.TransactionStatus)
                .Include(t => t.PaymentType)
                .ToList();
        }
    }
}
