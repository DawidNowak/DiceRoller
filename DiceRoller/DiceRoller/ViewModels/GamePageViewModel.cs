using System.Collections.ObjectModel;
using System.Linq;
using DiceRoller.DataAccess.Models;
using DiceRoller.Interfaces;
using Prism.Commands;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
    public class GamePageViewModel : ViewModelBase
    {
        public IView View { get; set; }
        public DelegateCommand IncreaseDiceCommand { get; }
        public DelegateCommand DecreaseDiceCommand { get; }

        public GamePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Game View Model";
            IncreaseDiceCommand = new DelegateCommand(increaseDice);
            DecreaseDiceCommand = new DelegateCommand(decreaseDice);
        }

        private Game _game;

        public Game Game
        {
            get => _game;
            set => SetProperty(ref _game, value);
        }

        private int _diceNumber = 1;

        public int DiceNumber
        {
            get => _diceNumber;
            set
            {
                if (0 < value && value < 9)
                    SetProperty(ref _diceNumber, value);
            }
        }

        public ObservableCollection<string> DiceMinis { get; set; }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            Game = (Game)parameters["game"];
            DiceMinis = new ObservableCollection<string>(Game.Dice.Select(d => d.MiniImageSource));
            View?.PopulateMinis();
            base.OnNavigatedTo(parameters);
        }

        private void increaseDice()
        {
            DiceNumber++;
            View?.AddDice();
        }

        private void decreaseDice()
        {
            DiceNumber--;
            View?.RemoveDice();
        }
    }
}
