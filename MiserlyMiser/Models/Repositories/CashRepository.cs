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
    public class CashRepository : DefaultCrudRepository<Cash>, ICrudRepository<Cash>
    {
        public CashRepository(MiserlyMiserDataContext context) : base(context)
        {
        }

        public override ICollection<Cash> GetAll()
        {
            return EntitySet
                .Include(cash => cash.Currency)
                .Include(cash => cash.CashType)
                .ToList();
        }
    }
}
