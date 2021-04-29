using System;
using System.Windows.Data;

namespace Quotes.Tools
{
    //Конвертер решает проблему исчезновения точки при привязке дроби. Взято отсюда https://stackoverflow.com/questions/11223236/binding-to-double-field-with-validation
    //A clever converter that remembers the last string converted to double and returns that if it exists should do everything you want.
    //Note that when the user changes the contents of the textbox, ConvertBack will store the string the user input, parse the string for a double, and pass that value to the view model.Immediately after, Convert is called to display the newly changed value.At this point, the stored string is not null and will be returned.
    //If the application instead of the user causes the double to change only Convert is called.This means that the cached string will be null and a standard ToString() will be called on the double.
    //In this way, the user avoids strange surprises when modifying the contents of the textbox but the application can still trigger a change.

    public class DoubleToPersistentStringConverter : IValueConverter
    {
        private string _lastConvertBackString;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is double)) return null;

            var stringValue = _lastConvertBackString ?? value.ToString();
            _lastConvertBackString = null;

            return stringValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is string)) return null;

            if (double.TryParse((string)value, out var result))
            {
                _lastConvertBackString = (string)value;
                return result;
            }

            return null;
        }
    }
}
