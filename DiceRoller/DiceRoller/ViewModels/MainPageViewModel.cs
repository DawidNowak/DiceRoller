using System.Collections.Generic;
using System.Collections.ObjectModel;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace DiceRoller.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IContext _ctx;

        public MainPageViewModel(INavigationService navigationService, IContext ctx) 
            : base (navigationService)
        {
            _ctx = ctx;
            Title = "Main Page";
            Games = new ObservableCollection<Game>();
            ctx.GetAll<Game>().ForEach(g => Games.Add(g));
        }

        public ObservableCollection<Game> Games { get; set; }
    }
}
