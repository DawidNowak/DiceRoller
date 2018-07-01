using System.IO;
using Android.Graphics;
using Com.Theartofdev.Edmodo.Cropper;
using DiceRoller.Controls;
using DiceRoller.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CropView), typeof(CropViewRenderer))]
namespace DiceRoller.Droid.CustomRenderers
{
#pragma warning disable 618
	public class CropViewRenderer : PageRenderer
#pragma warning restore 618
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged(e);
			if (Element is CropView page)
			{
				var cropImageView = new CropImageView(Context);
				cropImageView.LayoutParameters = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
				Bitmap bitmp = BitmapFactory.DecodeByteArray(page.Image, 0, page.Image.Length);
				cropImageView.SetImageBitmap(bitmp);

				var scrollView = new ScrollView { Content = cropImageView.ToView() };
				var stackLayout = new StackLayout { Children = { scrollView } };

				var rotateButton = new Button { Text = "Rotate" };

				rotateButton.Clicked += (sender, ex) =>
				{
					cropImageView.RotateImage(90);
				};
				stackLayout.Children.Add(rotateButton);

				var finishButton = new Button { Text = "Finished" };
				finishButton.Clicked += (sender, ex) =>
				{
					Bitmap cropped = cropImageView.CroppedImage;
					using (MemoryStream memory = new MemoryStream())
					{
						cropped.Compress(Bitmap.CompressFormat.Png, 100, memory);
						App.CroppedImage = memory.ToArray();
					}
					page.DidCrop = true;
					App.MasterDetail.Detail.Navigation.PopAsync(true);
				};

				stackLayout.Children.Add(finishButton);
				page.Content = stackLayout;
			}
		}
	}
}