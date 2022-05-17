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
    public class CategoryRepository : DefaultCrudRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(MiserlyMiserDataContext context) : base(context)
        {
        }

        public override ICollection<Category> GetAll()
        {
            return EntitySet
                .Include(c => c.Parent)
                .ThenInclude(p => p.CategoryCharacter)
                .Include(c => c.ChildCategories)
                .Include(c => c.CategoryCharacter)
                .ToArray();
        }

        public override Category GetById(int id)
        {
            return EntitySet
                .Include(c => c.Parent)
                .Include(c => c.ChildCategories)
                .Include(c => c.CategoryCharacter)
                .FirstOrDefault(c => c.Id == id);
        }
    }
}
