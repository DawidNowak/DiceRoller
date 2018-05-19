using System.Collections.Generic;
using System.Linq;
using DiceRoller.DataAccess.Models;
using DiceRoller.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;

namespace DiceRoller.ViewModels
{
    public class GamePageViewModel : ViewModelBase
    {
        private readonly ICollection<View> _minis;

        public IView View { get; set; }
        public DelegateCommand<Dice> IncreaseDiceCommand { get; }

        public GamePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Game View Model";
            IncreaseDiceCommand = new DelegateCommand<Dice>(AddDice);
            _minis = new List<View>();
        }

        private Game _game;

        public Game Game
        {
            get => _game;
            set => SetProperty(ref _game, value);
        }

        private int _diceNumber;

        public int DiceNumber
        {
            get => _diceNumber;
            set => SetProperty(ref _diceNumber, value);
        }

        public void AddDice(Dice toAdd)
        {
            if (_diceNumber >= 8) return;

            DiceNumber++;
            View?.AddDice(toAdd);
        }

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            Game = (Game)parameters["game"];
            PopulateDiceMinis();
            base.OnNavigatedTo(parameters);
        }

        private void PopulateDiceMinis()
        {
            var minis = Game.Dice.Select(d => d.MiniImageSource).ToArray();

            for (int i = 0; i < minis.Length; i++)
            {
                var fullPath = $"{Game.Dice.ElementAt(i).Path}{minis[i]}";
                var img = new Image
                {
                    Source = ImageSource.FromResource(fullPath),
                    BindingContext = Game.Dice.ElementAt(i),
                    Scale = 2.0
                };

#pragma warning disable CS0618 // Type or member is obsolete
                img.GestureRecognizers.Add(new TapGestureRecognizer(view =>
                {
                    AddDice((Dice)((Image)view).Source.BindingContext);
                }));
#pragma warning restore CS0618 // Type or member is obsolete

                _minis.Add(img);
            }

            View?.RefreshMinis(_minis);
        }
    }
}
