using System;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Interfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace DiceRoller.ViewModels
{
	public class DiceCreatorPageViewModel : ViewModelBase
	{
		private IContext _ctx;

		public IDiceCreatorView View;

		public DiceCreatorPageViewModel(INavigationService navigationService, IContext ctx) : base(navigationService)
		{
			_ctx = ctx;
			SetMiniImageCommand = new DelegateCommand(SetImage);
		}

		public DelegateCommand SetMiniImageCommand { get; set; }

		private Dice _dice;

		public void SetDice(Dice dice)
		{
			_dice = dice;
			Path = _dice.Path.Replace(". Mini image not set.", "");
		}

		private string _path;

		public string Path
		{
			get => _path;
			set => SetProperty(ref _path, value);
		}

		private ImageSource _miniImageSource;

		public ImageSource MiniImageSource
		{
			get => _miniImageSource;
			set => SetProperty(ref _miniImageSource, value);
		}

		private async void SetImage()
		{
			if (await View.ImageSourceAlert())  //File
			{

			}
			else //Camera
			{
				await CrossMedia.Current.Initialize();

				if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
				{
					//View.DisplayAlert("No Camera", "No camera available.", "OK");
					return;
				}

				var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
				{
					AllowCropping = true,
					DefaultCamera = CameraDevice.Rear,
					SaveToAlbum = false,
					Name = "test.jpg"
				});

				if (file == null)
					return;

				MiniImageSource = ImageSource.FromStream(() =>
				{
					var stream = file.GetStream();
					return stream;
				});
			}
		}
	}
}
