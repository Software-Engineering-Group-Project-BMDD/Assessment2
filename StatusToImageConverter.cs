using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public class StatusToImageConverter : IValueConverter
    {
        private static readonly Random RandomGenerator = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Randomly return one of the two icons
            return RandomGenerator.Next(2) == 0 ? "wifi_icon_online.png" : "wifi_icon_offline.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
