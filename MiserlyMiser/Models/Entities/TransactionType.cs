using MiserlyMiser.Models.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Models.Entities
{
    public enum TransactionType
    {
        [Description("Доход")]
        Incoming,

        [Description("Расход")]
        Spending,

        [Description("Перевод")]
        Transfer
    }

    public static class TransactionTypeExtensions
    {
        public static string Name(this TransactionType type)
            => type.GetType().GetMember(type.ToString()).First().GetCustomAttribute<DescriptionAttribute>().Description;
        
    }
}
