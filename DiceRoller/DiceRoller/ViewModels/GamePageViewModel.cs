using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Helpers;
using DiceRoller.DataAccess.Models;
using DiceRoller.Extensions;
using DiceRoller.Helpers;
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

		public IGameView View { get; set; }
		public DelegateCommand RollCommand { get; }


		public GamePageViewModel(IContext ctx, INavigationService navigationService, IEventAgregator eventAgregator) : base(navigationService)
		{
			_ctx = ctx;
			Title = "Game View Model";
			RollCommand = new DelegateCommand(Roll, CanRoll);
			_minis = new List<View>();
			eventAgregator.Subscribe<AnimateRollChanged>(o => _animateRoll = (bool)o);
			eventAgregator.Subscribe<SaveStateChanged>(o => _saveState = (bool)o);
		}

		private bool CanRoll() => _canRoll;

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

		private void PopulateDiceMinis()
		{
			Game.Dice.ForEach(d =>
			{
				var img = ImageHelper.DrawMini(d);

#pragma warning disable CS0618 // Type or member is obsolete
				img.GestureRecognizers.Add(new TapGestureRecognizer(view =>
				{
					AddDice((Dice)((Image)view).Source.BindingContext);
				}));
#pragma warning restore CS0618 // Type or member is obsolete

				_minis.Add(img);
			});

			View?.RefreshMinis(_minis);
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
					Thread.Sleep(5);
				}
			}
			else await FlipDice(dice, rand);
		}

		private static async Task FlipDice(IEnumerable<View> dice, Random rand, int delay = 0)
		{
			dice.ForEach(async d =>
			{
				var diceCtx = (Dice)d.BindingContext;
				await d.FadeTo(0, 50);

				if (!diceCtx.IsGenerated)
				{
					((SwipeableImage)d).Source = BlobHelper.GetImgSource(diceCtx.Walls.ElementAt(rand.Next(0, diceCtx.Walls.Count)).Image);
				}
				else
				{
					var diceData = new DiceData(diceCtx.Path);
					var skData = DrawHelper.DrawDice(diceData);
					((SwipeableImage)d).Source = ImageSource.FromStream(() => skData.AsStream());
				}

				await d.FadeTo(1, 50);
			});
			await Task.Delay(delay);
		}

		private void ChangeRollEnabled(bool canRoll)
		{
			_canRoll = canRoll;
			RollCommand.RaiseCanExecuteChanged();
		}

		public void AddDice(Dice toAdd)
		{
			if (_diceNumber >= 12) return;


			DiceNumber++;
			View?.AddDice(toAdd);
		}

		public void SetView()
		{
			PopulateDiceMinis();
			var configs = _ctx.GetAll<Config>();
			_saveState = configs.First(x => x.Key == Consts.SaveDiceStateKey).Value.ToBoolean();
			_animateRoll = configs.First(x => x.Key == Consts.RollAnimationKey).Value.ToBoolean();
			if (_saveState) LoadDiceSet();
		}

		public void OnDisappearing()
		{
			if (!_saveState) return;

			var setIds = string.Join(Consts.IdsSeparator.ToString(), View.Dice.Select(d => ((Dice)d.BindingContext).Id.ToString()));
			Game.DiceSet = setIds;
			_ctx.InsertOrReplace(Game);
		}
	}
}
