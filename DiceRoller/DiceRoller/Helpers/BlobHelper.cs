using System.IO;
using System.Reflection;
using Xamarin.Forms;

namespace DiceRoller.Helpers
{
    public static class BlobHelper
    {
        public static byte[] GetBytes(string imgName)
        {
			var assembly = typeof(BlobHelper).GetTypeInfo().Assembly;
            byte[] buffer;
            using (var s = assembly.GetManifestResourceStream(imgName))
            {
                if (s == null) return null;

                var length = s.Length;
                buffer = new byte[length];
                s.Read(buffer, 0, (int)length);
            }

            return buffer;
        }

        public static ImageSource GetImgSource(byte[] bytes)
        {
            return ImageSource.FromStream(() => new MemoryStream(bytes));
        }
    }
}
