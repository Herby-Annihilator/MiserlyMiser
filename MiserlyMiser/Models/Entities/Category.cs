using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
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
