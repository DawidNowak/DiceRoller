using DiceRoller.DataAccess.Models;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
    public class GamePageViewModel : ViewModelBase
    {
        private Game _game;

        public Game Game
        {
            get => _game;
            set => SetProperty(ref _game, value);
        }
        public GamePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Game View Model";
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            Game = (Game)parameters["game"];
            base.OnNavigatedTo(parameters);
        }
    }
}
