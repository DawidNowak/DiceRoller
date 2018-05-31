using System;
using System.Globalization;
using DiceRoller.DataAccess.Models;
using Xamarin.Forms;

namespace DiceRoller.Converters
{
    public class GameToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return string.Empty;

            var game = (Game)value;
            return ImageSource.FromResource($"{game.Path}{game.LogoImageSource}");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
