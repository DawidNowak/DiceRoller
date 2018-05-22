using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Models;
using DiceRoller.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DiceRoller.ViewModels
{
    public class GamePageViewModel : ViewModelBase
    {
        private bool _canRoll = true;
        private readonly ICollection<View> _minis;

        public IView View { get; set; }
        public DelegateCommand RollCommand { get; }


        public GamePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Game View Model";
            RollCommand = new DelegateCommand(Roll, CanRoll);
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

        private async void Roll()
        {
            ChangeRollEnabled(false);
            var dice = View?.Dice;
            var rand = new Random();

            for (int delay = 160; delay < 520; delay+=40)
            {
                dice.ForEach(async d =>
                {
                    var diceCtx = (Dice)d.BindingContext;
                    if (rand.Next(0, 4) == 0)
                    {
                        await Task.Delay(100);
                    }
                    else
                    {
                        await d.FadeTo(0, 50);
                        ((SwipeableImage)d).Source = ImageSource.FromResource(diceCtx.Path +
                                                                              diceCtx.Walls.ElementAt(rand.Next(0, diceCtx.Walls.Count))
                                                                                  .ImageSource);
                        await d.FadeTo(1, 50);
                    }

                    
                });
                await Task.Delay(delay);
            }

            ChangeRollEnabled(true);
        }

        private void ChangeRollEnabled(bool canRoll)
        {
            _canRoll = canRoll;
            RollCommand.RaiseCanExecuteChanged();
        }

        private bool CanRoll() => _canRoll;

        public void AddDice(Dice toAdd)
        {
            if (_diceNumber >= 12) return;

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
                    HeightRequest = 36d,
                    WidthRequest = 36d
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
