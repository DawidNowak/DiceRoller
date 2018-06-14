using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using DiceRoller.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace DiceRoller.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IContext _ctx;
        private readonly IEventAgregator _eventAggregator;
        private IList<Game> _games = new List<Game>();

        public ObservableCollection<Game> Games { get; set; } = new ObservableCollection<Game>();

        public DelegateCommand<Game> GameNavigationCommand { get; }

        public MainPageViewModel(INavigationService navigationService, IContext ctx, IEventAgregator eventAggregator)
            : base(navigationService)
        {
            _ctx = ctx;
            _eventAggregator = eventAggregator;
            Title = "Main Page";

            GameNavigationCommand = new DelegateCommand<Game>(Navigate);
            _eventAggregator.Subscribe<GameChangedEvent>(RefreshGames);
            RefreshGames();
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

        private void RefreshGames()
        {
            _games.Clear();
            Games.Clear();
            _ctx.GetAll<Game>().OrderBy(g => g.Name).ForEach(g =>
            {
                _games.Add(g);
                Games.Add(g);
            });
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            FilterText = string.Empty;
            base.OnNavigatedFrom(parameters);
        }

        private void Navigate(Game game)
        {
            var vm = new GamePageViewModel(_ctx, NavigationService) {Game = game};
            var page = new GamePage { BindingContext = vm };

            App.MasterDetail.Detail.Navigation.PushAsync(page);
            vm.SetView();
        }
    }
}
