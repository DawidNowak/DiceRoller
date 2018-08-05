using System;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using DiceRoller.Controls;
using DiceRoller.Interfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace DiceRoller.Helpers
{
	public static class CameraHelper
	{

		//TODO: LOWER PICTURES RESOLUTION, A LOT!!! APP CRASHES AND PERFORMANCE IS TRAGIC
		public static async Task<byte[]> TakePhoto(Action refresh)
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
				PhotoSize = PhotoSize.Small,
				SaveToAlbum = false,
				RotateImage = false,
				CustomPhotoSize = 50
			});

			return await SetPhoto(file, refresh);
		}

		private static byte[] FlipToPortrait(MediaFile file)
		{
			var options = new BitmapFactory.Options { InJustDecodeBounds = false };
			var bitmap = BitmapFactory.DecodeFile(file.Path, options);

			var mtx = new Matrix();
			mtx.PreRotate(90);
			bitmap = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, mtx, false);
			mtx.Dispose();

			byte[] bitmapData;
			using (var stream = new MemoryStream())
			{
				bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
				bitmapData = stream.ToArray();
			}

			return bitmapData;
		}

		public static async Task<byte[]> PickPhoto(Action refresh)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsPickPhotoSupported)
			{
				//TODO: DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
				return null;
			}
			var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
			{
				PhotoSize = PhotoSize.Medium,
				RotateImage = false
			});

			return await SetPhoto(file, refresh);
		}

		private static async Task<byte[]> SetPhoto(MediaFile file, Action refresh)
		{
			byte[] byteArr;
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
	}
}
