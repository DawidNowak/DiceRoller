using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using DiceRoller.Interfaces;
using DiceRoller.Views;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DiceRoller.ViewModels
{
	public class GameCreatorPageViewModel : SaveableBaseViewModel<Game>
	{
		private readonly IContext _ctx;
		private readonly IEventAgregator _eventAggregator;

		public GameCreatorPageViewModel(IContext ctx, INavigationService navigationService, IEventAgregator eventAggregator) : base(navigationService)
		{
			_ctx = ctx;
			Title = "Game Creator";

			_eventAggregator = eventAggregator;
			DiceList = new ObservableCollection<Dice>();
			AddDiceCommand = new DelegateCommand(AddDice);
			EditDiceCommand = new DelegateCommand<Dice>(EditDice);
			DeleteDiceCommand = new DelegateCommand<Dice>(DeleteDice);
			SetLogoImageCommand = new DelegateCommand(SetLogoImage);

			Model = new Game
			{
				Id = _ctx.GetNextId<Game>(),
				IsEditable = true,
				Dice = new List<Dice>()
			};
		}

		public DelegateCommand AddDiceCommand { get; set; }
		public DelegateCommand<Dice> EditDiceCommand { get; set; }
		public DelegateCommand<Dice> DeleteDiceCommand { get; set; }
		public DelegateCommand SetLogoImageCommand { get; set; }

		private string _name;
		public string Name
		{
			get => _name;
			set
			{
				SetProperty(ref _name, value);
				Model.Name = value;
				SaveCommand.RaiseCanExecuteChanged();
			}
		}

		private byte[] _logoImgBytes;
		public byte[] LogoImgBytes
		{
			get => _logoImgBytes;
			set
			{
				SetProperty(ref _logoImgBytes, value);
				Model.LogoImage = value;
				SaveCommand.RaiseCanExecuteChanged();
			} 
		}

		private ObservableCollection<Dice> _diceList;

		public ObservableCollection<Dice> DiceList
		{
			get => _diceList;
			set => SetProperty(ref _diceList, value);
		}

		protected override async void save()
		{
			_ctx.InsertOrReplace(Model);
			_eventAggregator.Publish<GameChangedEvent>();
			await App.MasterDetail.Detail.Navigation.PopAsync();
		}

		protected override bool canSave()
		{
			return !string.IsNullOrEmpty(Name) && LogoImgBytes != null && DiceList.Count > 0;
		}

		public void SetModel(Game game)
		{
			Model = game;
			Name = game.Name;
			LogoImgBytes = game.LogoImage;
			RefreshGame();
		}

		private async void AddDice()
		{
			var type = await ((IGameCreatorView)View).DiceTypeAlert();
			var dice = new Dice
			{
				Id = _ctx.GetNextId<Dice>(),
				Game = Model,
				GameId = Model.Id,
				IsGenerated = type,
				Path = type ? "Generated" : "Picture" + $" Dice no.{DiceList.Count + 1}. Mini image not set.",
				Walls = new ObservableCollection<DiceWall>()
			};
			DiceList.Add(dice);
			Model.Dice.ToList().Add(dice);
			EditDice(dice);
			SaveCommand.RaiseCanExecuteChanged();
		}

		private void EditDice(Dice dice)
		{
			ContentPage page;
			if (dice.IsGenerated)
			{
				var vm = new GenDiceCreatorPageViewModel(NavigationService, _ctx);
				vm.SetModel(dice);
				vm.RefreshGame = RefreshGame;
				page = new GenDiceCreatorPage {BindingContext = vm};
			}
			else
			{
				var vm = new DiceCreatorPageViewModel(NavigationService, _ctx);
				vm.SetModel(dice);
				vm.RefreshGame = RefreshGame;
				page = new DiceCreatorPage {BindingContext = vm};
			}

			App.MasterDetail.Detail.Navigation.PushAsync(page);
		}

		private async void DeleteDice(Dice dice)
		{
			if (await ((IGameCreatorView)View).DisplayAlert(dice.Path.Replace(". Mini image not set.", ""))) DiceList.Remove(dice);
		}

		private async void SetLogoImage()
		{
			if (await View.ImageSourceAlert("Logo image")) //file
			{
				LogoImgBytes = await CameraHelper.PickPhoto(RefreshLogo);
				if (LogoImgBytes.Length == 0) await PermissionDeniedPopup("storage");
			}
			else
			{
				LogoImgBytes = await CameraHelper.TakePhoto(RefreshLogo);
				if (LogoImgBytes.Length == 0) await PermissionDeniedPopup("camera");
			}
		}

		private void RefreshLogo()
		{
			if (App.CroppedImage != null)
			{
				LogoImgBytes = App.CroppedImage;
			}
		}

		private void RefreshGame()
		{
			DiceList.Clear();
			Model.Dice.ForEach(d =>
			{
				DiceList.Add(d);
			});
			SaveCommand.RaiseCanExecuteChanged();
		}
	}
}
