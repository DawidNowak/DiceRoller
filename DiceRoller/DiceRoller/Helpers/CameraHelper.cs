using System.IO;
using System.Threading.Tasks;
using Android;
using Android.Graphics;
using Android.Media;
using DiceRoller.Extensions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Image = Xamarin.Forms.Image;
using MediaOrientation = Android.Media.Orientation;

namespace DiceRoller.Helpers
{
	public static class CameraHelper
	{
		public static async Task<byte[]> TakePicture()
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				//View.DisplayAlert("No Camera", "No camera available.", "OK");
				return null;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
			{
				AllowCropping = true,
				DefaultCamera = CameraDevice.Front,
				SaveToAlbum = false,
				Name = "test.jpg",
				RotateImage = false,
				CustomPhotoSize = 50,
				CompressionQuality = 80
			});

			return file != null ? FlipToPortrait(file) : new byte[0];
		}

		private static byte[] FlipToPortrait(MediaFile file)
		{
			var options = new BitmapFactory.Options { InJustDecodeBounds = false };
			var bitmap = BitmapFactory.DecodeFile(file.Path, options);

			var mtx = new Matrix();
			mtx.PreRotate(90);
			bitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, mtx, false);
			mtx.Dispose();
			mtx = null;

			byte[] bitmapData;
			using (var stream = new MemoryStream())
			{
				bitmap.Compress(Bitmap.CompressFormat.Jpeg, 0, stream);
				bitmapData = stream.ToArray();
			}

			return bitmapData;
		}
	}
}
