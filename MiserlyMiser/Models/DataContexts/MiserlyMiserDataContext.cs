using Microsoft.EntityFrameworkCore;
using MiserlyMiser.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.DataContexts
{
    public class MiserlyMiserDataContext : DbContext
    {
        public MiserlyMiserDataContext() : base()
        {
        }

        #region DbSets

        public DbSet<Budget> Budgets { get; set; }
        public DbSet<Cash> Cashs { get; set; }
        public DbSet<CashType> CashTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryCharacter> CategoryCharacters { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<FinancialGoal> FinancialGoals { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Period> Periods { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionStatus> TransactionStatuses { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=MiserlyMiser;Username=postgres;Password=postgres");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
