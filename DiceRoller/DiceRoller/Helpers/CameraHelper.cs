using System;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.Graphics;
using Android.Media;
using DiceRoller.Controls;
using DiceRoller.Extensions;
using DiceRoller.Interfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Image = Xamarin.Forms.Image;
using MediaOrientation = Android.Media.Orientation;

namespace DiceRoller.Helpers
{
	public static class CameraHelper
	{
		public static async Task<byte[]> TakePicture(Action refresh)
		{
			byte[] byteArr;
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

			var orientation = DependencyService.Get<IDeviceOrientation>().GetOrientation();

			if (file != null)
			{
				if (orientation == DeviceOrientations.Portrait)
				{
					byteArr = FlipToPortrait(file);
				}
				else
				{
					using (var memStream = new MemoryStream())
					{
						file.GetStream().CopyTo(memStream);
						file.Dispose();
						byteArr = memStream.ToArray();
					}
				}

				await App.MasterDetail.Detail.Navigation.PushAsync(new CropView(byteArr, refresh));
				if (App.CroppedImage != null)
				{
					byteArr = App.CroppedImage;
				}
			}
			else byteArr = new byte[0];
			
			return byteArr;
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
				bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
				bitmapData = stream.ToArray();
			}

			return bitmapData;
		}
	}
}
