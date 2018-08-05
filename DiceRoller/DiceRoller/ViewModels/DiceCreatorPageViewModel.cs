using System;
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
	public class DiceCreatorPageViewModel : SaveableBaseViewModel<Dice>
	{
		private readonly IContext _ctx;
		private DiceWall _wall;

		public IDiceCreatorView View;

		public DiceCreatorPageViewModel(INavigationService navigationService, IContext ctx) : base(navigationService)
		{
			_ctx = ctx;
			SetMiniImageCommand = new DelegateCommand(SetImage);
			AddDiceWallCommand = new DelegateCommand(AddDiceWall);
			DiceWalls = new ObservableCollection<SwipeableImage>();
		}

		public Action RefreshGame { get; set; }
		public DelegateCommand SetMiniImageCommand { get; set; }
		public DelegateCommand AddDiceWallCommand { get; set; }

		public override void SetModel(Dice dice)
		{
			base.SetModel(dice);
			Path = Model.Path.Replace(". Mini image not set.", "");
			Model.Walls.ForEach(w =>
			{
				DiceWalls.Add(new SwipeableImage
				{
					Source = BlobHelper.GetImgSource(w.Image),
					HeightRequest = 36d,
					WidthRequest = 36d
				});
			});
			if (Model.MiniImage != null) MiniImageSource = BlobHelper.GetImgSource(Model.MiniImage);
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
			byte[] img;
			if (await View.ImageSourceAlert("Mini image")) //File
			{
				img = await CameraHelper.PickPhoto(RefreshMini);
			}
			else
			{
				img = await CameraHelper.TakePhoto(RefreshMini);
			}
			MiniImageSource = BlobHelper.GetImgSource(img);
			Model.MiniImage = img;
		}

		private async void AddDiceWall()
		{
			byte[] img;
			if (await View.ImageSourceAlert("Dice wall image")) //File
			{
				img = await CameraHelper.PickPhoto(RefreshWall);
			}
			else
			{
				img = await CameraHelper.TakePhoto(RefreshWall);
			}

			_wall = new DiceWall
			{
				Dice = Model,
				DiceId = Model.Id,
				Image = img
			};

			Model.Walls.Add(_wall);
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
			{
				Model.MiniImage = App.CroppedImage;
				MiniImageSource = BlobHelper.GetImgSource(App.CroppedImage);
			}
		}

		protected override async void save()
		{
			var nextId = _ctx.GetNextId<DiceWall>();
			Model.Walls.ForEach(w => w.Id = nextId++);
			RefreshGame?.Invoke();
			_ctx.InsertOrReplace(Model);
			Model.Walls.ForEach(w => _ctx.InsertOrReplace(w));
			await App.MasterDetail.Detail.Navigation.PopAsync();
		}

		protected override bool canSave()
		{
			return DiceWalls.Count > 1;
		}
	}
}