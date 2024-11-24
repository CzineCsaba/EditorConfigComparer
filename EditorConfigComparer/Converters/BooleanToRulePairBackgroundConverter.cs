using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace EditorConfigComparer.Converters
{
    public class BooleanToRulePairBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (True.Equals(value))
            {
                return Brushes.LightGreen;
            }

            return Brushes.LightSalmon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private static readonly object True = true;
        private static readonly object False = false;
    }
}
