using System;
using System.Globalization;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using Xamarin.Forms;

namespace DiceRoller.Converters
{
    public class GameToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var game = (Game)value;
            return BlobHelper.GetImgSource(game.LogoImage);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
