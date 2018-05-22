using System.Collections.ObjectModel;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace DiceRoller.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public ObservableCollection<Game> Games { get; set; }

        public DelegateCommand<Game> GameNavigationCommand { get; }
        public DelegateCommand SettingsNavigationCommand { get; }
        
        public MainPageViewModel(INavigationService navigationService, IContext ctx) 
            : base (navigationService)
        {
            Title = "Main Page";

            GameNavigationCommand = new DelegateCommand<Game>(navigate);
            SettingsNavigationCommand = new DelegateCommand(navigateToSettings);

            Games = new ObservableCollection<Game>();
            ctx.GetAll<Game>().ForEach(g => Games.Add(g));
        }

        private async void navigate(Game game)
        {
            var param = new NavigationParameters {{"game", game}};
            await NavigationService.NavigateAsync("GamePage", param);
        }

        private async void navigateToSettings()
        {
            await NavigationService.NavigateAsync("SettingsPage");
        }
    }
}
