using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Tema3_Restaurant.ViewModels
{
    public class OrderStateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is string state)
            {
                return state.ToLower() switch
                {
                    "inregistrata" => new SolidColorBrush(Colors.DodgerBlue),
                    "se pregateste" => new SolidColorBrush(Colors.Orange),
                    "a plecat la client" => new SolidColorBrush(Colors.Purple),
                    "livrata" => new SolidColorBrush(Colors.Green),
                    "anulata" => new SolidColorBrush(Colors.Red),
                    _ => new SolidColorBrush(Colors.Gray)

                };

            }
            return new SolidColorBrush(Colors.Gray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
