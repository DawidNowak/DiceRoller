using DiceRoller.DataAccess.Models;
using Prism.Commands;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
    public class GamePageViewModel : ViewModelBase
    {
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

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            Game = (Game)parameters["game"];
            base.OnNavigatedTo(parameters);
        }

        private void increaseDice()
        {
            DiceNumber++;
        }

        private void decreaseDice()
        {
            DiceNumber--;
        }
    }
}
