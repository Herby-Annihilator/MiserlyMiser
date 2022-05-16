using MiserlyMiser.Infrastructure.Converters.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiserlyMiser.Infrastructure.Converters
{
    public class DateTimeToStringFormatConverter : Converter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is DateTime date)
            {
                return $"{date.Day}. {date.Month}. {date.Year}";
            }
            else
                return "";
        }
    }
}
