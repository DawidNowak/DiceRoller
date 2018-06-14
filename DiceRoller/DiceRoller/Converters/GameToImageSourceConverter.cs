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
            return game.LogoImage != null
                ? BlobHelper.GetImgSource(game.LogoImage)
                //TODO: CHECK WHY TEXT IS NOT APPEARING AND 300X100 RECT IS SQUAGE >.<
                : ImageSource.FromStream(() => DrawHelper.DrawText(game.Name, 300f, 100f).AsStream());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
