using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Entities
{
    public class Category : NamedEntity
    {
        public Category? Parent { get; set; }
        public int? ParentId { get; set; }

        public ICollection<Category>? ChildCategories { get; set; }

        public ICollection<Transaction>? Transactions { get; set; }

        public CategoryCharacter CategoryCharacter { get; set; }
        public int CategoryCharacterId { get; set; }

        public ICollection<Budget> Budgets { get; set; }
    }
}
