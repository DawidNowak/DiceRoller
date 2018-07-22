using System.Collections.Generic;
using System.Collections.ObjectModel;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using DiceRoller.Interfaces;
using DiceRoller.Views;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace DiceRoller.ViewModels
{
	public class GameCreatorPageViewModel : ViewModelBase
	{
		private readonly IContext _ctx;
		private readonly IEventAgregator _eventAggregator;
		private Game _game;

		public GameCreatorPageViewModel(IContext ctx, INavigationService navigationService, IEventAgregator eventAggregator) : base(navigationService)
		{
			_ctx = ctx;
			Title = "Game Creator";

			_eventAggregator = eventAggregator;
			DiceList = new ObservableCollection<Dice>();
			SaveCommand = new DelegateCommand(Save, CanSave);
			AddDiceCommand = new DelegateCommand(AddDice);
			EditDiceCommand = new DelegateCommand<Dice>(EditDice);
			DeleteDiceCommand = new DelegateCommand<Dice>(DeleteDice);
			SetLogoImageCommand = new DelegateCommand(SetLogoImage);

			_game = new Game
			{
				Id = _ctx.GetNextId<Game>(),
				IsEditable = true,
				Dice = new List<Dice>()
			};
		}

		public IGameCreatorView View { get; set; }

		public DelegateCommand SaveCommand { get; set; }
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
				_game.Name = value;
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
				_game.LogoImage = value;
			} 
		}

		private ObservableCollection<Dice> _diceList;

		public ObservableCollection<Dice> DiceList
		{
			get => _diceList;
			set => SetProperty(ref _diceList, value);
		}

		private async void Save()
		{
			_ctx.InsertOrReplace(_game);
			_eventAggregator.Publish<GameChangedEvent>();
			await App.MasterDetail.Detail.Navigation.PopAsync();
		}

		private bool CanSave()
		{
			return !string.IsNullOrEmpty(Name) && LogoImgBytes != null;
		}

		private void AddDice()
		{
			var dice = new Dice
			{
				Id = _ctx.GetNextId<Dice>(),
				Game = _game,
				GameId = _game.Id,
				Path = $"Dice no.{DiceList.Count + 1}. Mini image not set.",
				Walls = new ObservableCollection<DiceWall>()
			};
			DiceList.Add(dice);
			_game.Dice.Add(dice);
		}

		private void EditDice(Dice dice)
		{
			var vm = new DiceCreatorPageViewModel(NavigationService, _ctx);
			vm.SetDice(dice);
			vm.RefreshGame = RefreshGame;
			var page = new DiceCreatorPage { BindingContext = vm };

			App.MasterDetail.Detail.Navigation.PushAsync(page);
		}

		private async void DeleteDice(Dice dice)
		{
			if (await View.DisplayAlert(dice.Path.Replace(". Mini image not set.", ""))) DiceList.Remove(dice);
		}

		private async void SetLogoImage()
		{
			if (await View.ImageSourceAlert("Logo image"))  //file
			{
				LogoImgBytes = await CameraHelper.PickPhoto(RefreshLogo);
			}
			else LogoImgBytes = await CameraHelper.TakePhoto(RefreshLogo);
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
			_game.Dice.ForEach(d =>
			{
				DiceList.Add(d);
			});
			SaveCommand.RaiseCanExecuteChanged();
		}
	}
}
