﻿using System.Collections.ObjectModel;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using DiceRoller.Interfaces;
using DiceRoller.Views;
using Prism.Commands;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
    public class GameCreatorPageViewModel : ViewModelBase
    {
        private readonly IContext _ctx;
        private readonly IEventAgregator _eventAggregator;

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
        }

        public IGameCreatorView View { get; set; }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand AddDiceCommand { get; set; }
        public DelegateCommand<Dice> EditDiceCommand { get; set; }
        public DelegateCommand<Dice> DeleteDiceCommand { get; set; }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private byte[] _logoImgBytes;
        public byte[] LogoImgBytes
        {
            get => _logoImgBytes;
            set => SetProperty(ref _logoImgBytes, value);
        }

        private ObservableCollection<Dice> _dice;

        public ObservableCollection<Dice> DiceList
        {
            get => _dice;
            set => SetProperty(ref _dice, value);
        }

        private void Save()
        {
            var game = new Game
            {
                Id = _ctx.GetNextId<Game>(),
                IsEditable = true,
                Name = Name,
                LogoImage = LogoImgBytes
            };

            _ctx.InsertOrReplace(game);
            _eventAggregator.Publish<GameChangedEvent>();
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Name) && LogoImgBytes != null;
        }

        private void AddDice()
        {
            DiceList.Add(new Dice { Path = $"Dice no.{DiceList.Count + 1}. Mini image not set." });
        }

        private void EditDice(Dice dice)
        {
            var vm = new DiceCreatorPageViewModel(NavigationService, _ctx);
            vm.SetDice(dice);
            var page = new DiceCreatorPage { BindingContext = vm };

            App.MasterDetail.Detail.Navigation.PushAsync(page);
        }

        private async void DeleteDice(Dice dice)
        {
            if (await View.DisplayAlert(dice.Path.Replace(". Mini image not set.", ""))) DiceList.Remove(dice);
        }
    }
}
