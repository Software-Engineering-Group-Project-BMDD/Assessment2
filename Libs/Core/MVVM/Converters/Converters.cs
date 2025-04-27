using System.Globalization;

// this is basically a hand helper or utility class, i hae left this here inside the class library for ease and access
namespace MauiApp1.Libs.Core.MVVM.Converters
{
    // for hiding and showing labels if no data
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Return true if value is not null, false otherwise
            return value != null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // this flip true and false and vice versa
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Invert the boolean value
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Also invert when converting back
            if (value is bool boolValue)
            {
                return !boolValue;
            }
            return false;
        }
    }

    // this is for checking against o values in a list 
    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Converts integer > 0 to true, otherwise false
            return value is int count && count > 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    // this is for directional conversion from a numeral degress, compass 
    public class WindDirectionToCardinalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int degrees)
            {
                string[] cardinals = ["N", "NE", "E", "SE", "S", "SW", "W", "NW"];
                return cardinals[(int)Math.Round(degrees % 360 / 45.0) % 8];
            }
            return "N/A";
        }


        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            // we could convert back but would like accuracy due to conformity
            throw new NotImplementedException();
        }
    }

    // similar as int to bool for visibility    
    public class IntToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => (value is int count && count > 0);


        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
