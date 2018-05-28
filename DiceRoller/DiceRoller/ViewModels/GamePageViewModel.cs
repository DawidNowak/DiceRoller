using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Helpers;
using DiceRoller.DataAccess.Models;
using DiceRoller.Extensions;
using DiceRoller.Interfaces;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DiceRoller.ViewModels
{
    public class GamePageViewModel : ViewModelBase
    {
        private readonly IContext _ctx;
        private bool _canRoll = true;
        private bool _animateRoll;
        private bool _saveState;
        private readonly ICollection<View> _minis;

        public IView View { get; set; }
        public DelegateCommand RollCommand { get; }


        public GamePageViewModel(IContext ctx, INavigationService navigationService) : base(navigationService)
        {
            _ctx = ctx;
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

        public override void OnNavigatedTo(NavigationParameters parameters)
        {
            Game = (Game)parameters["game"];
            PopulateDiceMinis();
            var configs = _ctx.GetAll<Config>();
            var animateKey = configs.First(x => x.Key == Consts.RollAnimationKey);
            _animateRoll = animateKey.Value.ToBoolean();
            var saveStateKey = configs.First(x => x.Key == Consts.SaveDiceStateKey);
            _saveState = saveStateKey.Value.ToBoolean();
            if (_saveState) LoadDiceSet();
            base.OnNavigatedTo(parameters);
        }

        private void PopulateDiceMinis()
        {
            var minis = Game.Dice.Select(d => d.MiniImageSource).ToArray();

            for (var i = 0; i < minis.Length; i++)
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

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            if (_saveState)
            {
                var setIds = string.Join(Consts.IdsSeparator.ToString(), View?.Dice.Select(d => ((Dice)d.BindingContext).Id.ToString()));
                Game.DiceSet = setIds;
                _ctx.InsertOrReplace(Game);
            }
            base.OnNavigatedFrom(parameters);
        }

        private void LoadDiceSet()
        {
            if (string.IsNullOrEmpty(Game.DiceSet)) return;

            var diceIds = Game.DiceSet.Split(Consts.IdsSeparator);
            diceIds.ForEach(id =>
            {
                var intId = Convert.ToInt32(id);
                var toAdd = _ctx.GetById<Dice>(intId);
                AddDice(toAdd);
            });
        }

        private async void Roll()
        {
            ChangeRollEnabled(false);
            await AnimateRoll();
            ChangeRollEnabled(true);
        }

        private async Task AnimateRoll()
        {
            var dice = View?.Dice;
            var rand = new Random();

            if (_animateRoll)
            {
                for (var delay = 160; delay < 520; delay += 40)
                {
                    await FlipDice(dice, rand, delay);
                }
            }
            else await FlipDice(dice, rand);
        }

        private static async Task FlipDice(IList<View> dice, Random rand, int delay = 0)
        {
            dice.ForEach(async d =>
            {
                var diceCtx = (Dice)d.BindingContext;
                await d.FadeTo(0, 50);
                ((SwipeableImage)d).Source = ImageSource.FromResource(diceCtx.Path +
                                                                      diceCtx.Walls.ElementAt(rand.Next(0, diceCtx.Walls.Count))
                                                                          .ImageSource);
                await d.FadeTo(1, 50);
            });
            await Task.Delay(delay);
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
    }
}
