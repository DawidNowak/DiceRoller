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

		    if (file != null)
		    {
				using (var memStream = new MemoryStream())
			    {
				    file.GetStream().CopyTo(memStream);
					file.Dispose();
				    byteArr = memStream.ToArray();
			    }
		    }
		    else byteArr = new byte[0];

			return byteArr;
	    }
	}
}
