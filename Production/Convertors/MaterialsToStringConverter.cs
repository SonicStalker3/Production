using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System;
using Production.DB;
using System.Diagnostics;
using System.Linq;

namespace Production 
{
    public class MaterialsToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<Material> materials)
            {
                return string.Join(", ", materials.Select(s => s.Name).DefaultIfEmpty("Не указано"));
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
