using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MauiApp1
{
    public class StatusToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string status)
            {
                return status switch
                {
                    "Online" => "wifi_icon_online.png", // Image for online status
                    "Offline" => "wifi_icon_offline.png", // Image for offline status
                    _ => "wifi_icon_unknown.png" // Default image for unknown status
                };
            }
            return "wifi_icon_unknown.png"; // Fallback image
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
