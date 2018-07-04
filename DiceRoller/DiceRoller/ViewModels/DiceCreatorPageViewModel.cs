using System.Collections.ObjectModel;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using DiceRoller.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DiceRoller.ViewModels
{
	public class DiceCreatorPageViewModel : ViewModelBase
	{
		private IContext _ctx;
		private DiceWall _wall;

		public IDiceCreatorView View;

		public DiceCreatorPageViewModel(INavigationService navigationService, IContext ctx) : base(navigationService)
		{
			_ctx = ctx;
			SetMiniImageCommand = new DelegateCommand(SetImage);
			AddDiceWallCommand = new DelegateCommand(AddDiceWall);
			SaveCommand = new DelegateCommand(Save);
			DiceWalls = new ObservableCollection<SwipeableImage>();
		}

		public DelegateCommand SetMiniImageCommand { get; set; }
		public DelegateCommand AddDiceWallCommand { get; set; }
		public DelegateCommand SaveCommand { get; set; }

		private Dice _dice;

		public void SetDice(Dice dice)
		{
			_dice = dice;
			Path = _dice.Path.Replace(". Mini image not set.", "");
			_dice.Walls.ForEach(w =>
			{
				DiceWalls.Add(new SwipeableImage
				{
					Source = BlobHelper.GetImgSource(w.Image),
					HeightRequest = 36d,
					WidthRequest = 36d
				});
			});
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

		private ObservableCollection<SwipeableImage> _diceWalls;

		public ObservableCollection<SwipeableImage> DiceWalls
		{
			get => _diceWalls;
			set => SetProperty(ref _diceWalls, value);
		}

		private async void SetImage()
		{
			if (await View.ImageSourceAlert())  //File
			{

			}
			else MiniImageSource = BlobHelper.GetImgSource(await CameraHelper.TakePicture(RefreshMini));
		}

		private async void AddDiceWall()
		{
			_wall = new DiceWall
			{
				Dice = _dice,
				DiceId = _dice.Id,
				Image = await CameraHelper.TakePicture(RefreshWall)
			};

			_dice.Walls.Add(_wall);
		}

		private void RefreshWall()
		{
			if (App.CroppedImage != null)
				_wall.Image = App.CroppedImage;

			var img = ImageHelper.DrawDiceWall(_wall, 64d);
			DiceWalls.Add(img);

			View.AddWall(img);
		}

		private void RefreshMini()
		{
			if (App.CroppedImage != null)
				MiniImageSource = BlobHelper.GetImgSource(App.CroppedImage);
		}

		private void Save()
		{

		}
	}
}
