using MiserlyMiser.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Repositories.Interfaces
{
    public interface ITransactionRepository : ICrudRepository<Transaction>
    {
    }
}
