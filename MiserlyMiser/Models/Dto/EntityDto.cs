using MiserlyMiser.Models.Entities.Base;
using MiserlyMiser.Models.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Dto
{
    public class EntityDto<T> where T : Entity
    {
        public string SemanticString { get; set; }
        public T Entity { get; set; }
        public ICrudRepository<T> Repository { get; set; }

        public EntityDto(string semanticString, T entity, ICrudRepository<T> repository)
        {
            SemanticString = semanticString;
            Entity = entity;
            Repository = repository;
        }
    }
}
