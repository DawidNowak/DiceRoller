using System.Collections.ObjectModel;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
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
            _eventAggregator = eventAggregator;
            DiceList = new ObservableCollection<Dice>();
            SaveCommand = new DelegateCommand(Save, CanSave);
            AddDiceCommand = new DelegateCommand(AddDice);
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand AddDiceCommand { get; set; }

        public string Title { get; set; } = "Game Creator";

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
    }
}
