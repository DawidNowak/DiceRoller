using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace DiceRoller.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IList<Game> _games;

        public ObservableCollection<Game> Games { get; set; } = new ObservableCollection<Game>();

        public DelegateCommand<Game> GameNavigationCommand { get; }
        public DelegateCommand InfoNavigationCommand { get; }

        public MainPageViewModel(INavigationService navigationService, IContext ctx)
            : base(navigationService)
        {
            Title = "Main Page";

            GameNavigationCommand = new DelegateCommand<Game>(navigate);
            InfoNavigationCommand = new DelegateCommand(navigateToInfo);

            _games = new List<Game>();
            ctx.GetAll<Game>().OrderBy(g => g.Name).ForEach(g =>
            {
                _games.Add(g);
                Games.Add(g);
            });
        }

        private string _filterText = string.Empty;

        public string FilterText
        {
            get => _filterText;
            set
            {
                SetProperty(ref _filterText, value);
                Games.Clear();
                if (_filterText.Length > 0) _games.Where(g => g.Name.ToLower().Contains(_filterText)).ForEach(g => Games.Add(g));
                else _games.ForEach(g => Games.Add(g));
            }
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            FilterText = string.Empty;
            base.OnNavigatedFrom(parameters);
        }

        private async void navigate(Game game)
        {
            var param = new NavigationParameters { { "game", game } };
            await NavigationService.NavigateAsync("GamePage", param);
        }

        private async void navigateToInfo()
        {
            await NavigationService.NavigateAsync("InfoPage");
        }
    }
}
