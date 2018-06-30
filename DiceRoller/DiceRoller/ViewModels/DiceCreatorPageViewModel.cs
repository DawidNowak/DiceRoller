using System;
using System.Collections.ObjectModel;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using DiceRoller.Interfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

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
			else MiniImageSource = BlobHelper.GetImgSource(await CameraHelper.TakePicture());
		}

		private async void AddDiceWall()
		{
			var wall = new DiceWall
			{
				Dice = _dice,
				DiceId = _dice.Id,
				Image = await CameraHelper.TakePicture()
			};
			_dice.Walls.Add(wall);

			var img = ImageHelper.DrawDiceWall(wall);
			DiceWalls.Add(img);

			View.AddWall(img);
		}

		private void Save()
		{
			
		}
	}
}
