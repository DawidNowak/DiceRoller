using System;
using System.Globalization;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using Xamarin.Forms;

namespace DiceRoller.Converters
{
    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var arr = (byte[])value;
            return BlobHelper.GetImgSource(arr);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
