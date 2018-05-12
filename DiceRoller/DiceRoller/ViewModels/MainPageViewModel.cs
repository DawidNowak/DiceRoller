using System.Collections.Generic;
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
        private readonly IContext _ctx;
        public ObservableCollection<Game> Games { get; set; }

        public DelegateCommand<Game> GameNavigationCommand { get; }
        public MainPageViewModel(INavigationService navigationService, IContext ctx) 
            : base (navigationService)
        {
            _ctx = ctx;
            Title = "Main Page";

            GameNavigationCommand = new DelegateCommand<Game>(Navigate);

            Games = new ObservableCollection<Game>();
            ctx.GetAll<Game>().ForEach(g => Games.Add(g));
        }

        private async void Navigate(Game game)
        {
            var param = new NavigationParameters {{"game", game}};

            await NavigationService.NavigateAsync("NavigationPage/GamePage", param);
        }
    }
}
