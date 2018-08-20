using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
		private int _nextId;

		public DiceCreatorPageViewModel(INavigationService navigationService, IContext ctx) : base(navigationService)
		{
			_ctx = ctx;
			_nextId = _ctx.GetNextId<DiceWall>();
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
			Refresh();
		}

		private void Refresh()
		{
			DiceWalls.Clear();
			Model.Walls.ForEach(w =>
			{
				var img = ImageHelper.DrawDiceWall(w, 64d);
				DiceWalls.Add(img);
				((IDiceCreatorView)View).AddWall(img);
			});
			if (Model.MiniImage != null) MiniImageSource = BlobHelper.GetImgSource(Model.MiniImage);
			SaveCommand.RaiseCanExecuteChanged();
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
			set
			{
				SetProperty(ref _miniImageSource, value);
				SaveCommand.RaiseCanExecuteChanged();
			}
		}

		private ObservableCollection<SwipeableImage> _diceWalls;
		public ObservableCollection<SwipeableImage> DiceWalls
		{
			get => _diceWalls;
			set => SetProperty(ref _diceWalls, value);
		}

		private async Task<byte[]> GetImage(string alert, Action refresh)
		{
			var img = new byte[0];
			if (await View.ImageSourceAlert(alert)) //File
			{
				img = await CameraHelper.PickPhoto(refresh);
				if (img.Length == 0) await PermissionDeniedPopup("storage");
			}
			else
			{
				img = await CameraHelper.TakePhoto(refresh);
				if (img.Length == 0) await PermissionDeniedPopup("camera");
			}

			return img;
		}

		private async void SetImage()
		{
			var img = await GetImage("Mini Image", RefreshMini);

			if (img.Length == 0)
			{
				MiniImageSource = BlobHelper.GetImgSource(img);
				Model.MiniImage = img;
			}
		}

		private async void AddDiceWall()
		{
			var img = await GetImage("Dice wall image", RefreshWall);
			if (img.Length == 0) return;

			_wall = new DiceWall
			{
				Dice = Model,
				DiceId = Model.Id,
				Image = img,
				Id = _nextId++
			};

			Model.Walls = new List<DiceWall>(Model.Walls) { _wall };
			if (DiceWalls.Count > 0) DiceWalls.RemoveAt(DiceWalls.Count - 1);
			DiceWalls.Add(ImageHelper.DrawDiceWall(_wall, 64d));
			SaveCommand.RaiseCanExecuteChanged();
		}

		public void DeleteDiceWall(DiceWall wall)
		{
			var walls = Model.Walls.ToList();
			walls.Remove(wall);
			Model.Walls = walls;
			_ctx.Delete(wall);
		}

		private void RefreshWall()
		{
			if (App.CroppedImage != null)
				_wall.Image = App.CroppedImage;

			var img = ImageHelper.DrawDiceWall(_wall, 64d);
			DiceWalls.Add(img);

			((IDiceCreatorView)View).AddWall(img);
		}

		private void RefreshMini()
		{
			if (App.CroppedImage == null) return;

			Model.MiniImage = App.CroppedImage;
			MiniImageSource = BlobHelper.GetImgSource(App.CroppedImage);
		}

		protected override async void save()
		{
			RefreshGame?.Invoke();
			Model.Walls.ForEach(w => _ctx.InsertOrReplace(w));
			await App.MasterDetail.Detail.Navigation.PopAsync();
		}

		protected override bool canSave()
		{
			if (Model == null) return false;
			Model.IsValid = MiniImageSource != null && DiceWalls.Count > 1;
			return Model.IsValid;
		}
	}
}