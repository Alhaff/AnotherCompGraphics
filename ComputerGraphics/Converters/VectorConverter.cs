using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace ComputerGraphics.Converters
{
    public class VectorToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return $"{((Vector)value).X:0.00} | {((Vector)value).Y:0.00}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = ((string)value).Split(new char[] { ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);
            float x = 0;
            float y = 0;
            if (str.Length > 0)
                float.TryParse(str[0], out x);
            if (str.Length > 1)
                float.TryParse(str[1], out y);
            return new Vector(x, y);

        }
    }
}
