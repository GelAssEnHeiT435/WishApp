using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WishListClient.src.Converters
{
    public class NullToPlaceholderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Debug.WriteLine($"Converter called: value={value}, param={parameter}"); // <-- Проверка

            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return parameter?.ToString() ?? "Описание отсутствует";
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value;
    }
}
