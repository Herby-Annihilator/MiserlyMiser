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
    public class FinancialGoalRepository : DefaultCrudRepository<FinancialGoal>, IFinancialGoalRepository
    {
        public FinancialGoalRepository(MiserlyMiserDataContext context) : base(context)
        {
        }

        public override ICollection<FinancialGoal> GetAll()
        {
            return EntitySet.Include(f => f.Period).ToList();
        }
    }
}
