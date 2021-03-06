﻿using System;
using System.Collections.Generic;
using System.Threading;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using DiceRoller.Interfaces;
using DiceRoller.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DiceRoller.Views
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamePage : ContentPage, IGameView
	{
		public GamePage()
		{
			InitializeComponent();
		}

		protected override void OnBindingContextChanged()
		{
			((GamePageViewModel)BindingContext).View = this;
			base.OnBindingContextChanged();
		}

		public IList<View> Dice => DiceLayout.Children;

		public void AddDice(Dice mini)
		{
			SwipeableImage diceImg;

			if (!mini.IsGenerated)
			{
				diceImg = ImageHelper.DrawDice(mini);
				Thread.Sleep(10);
			}
			else
			{
				var diceData = new DiceData(mini.Path);
				var skData = DrawHelper.DrawDice(diceData);

				diceImg = new SwipeableImage
				{
					Source = ImageSource.FromStream(() => skData.AsStream()),
					BindingContext = mini,
					HeightRequest = 64d,
					WidthRequest = 64d
				};
			}

			diceImg.SwipedLeft += (sender, args) => RemoveDice(DiceLayout.Children.IndexOf(sender));
			diceImg.SwipedRight += (sender, args) => RemoveDice(DiceLayout.Children.IndexOf(sender));

			DiceLayout.Children.Add(diceImg);
		}

		public void RemoveDice(int index)
		{
			if (index < 0) return;

			DiceLayout.Children.RemoveAt(index);
			((GamePageViewModel)BindingContext).DiceNumber--;
		}

		public void RefreshMinis(ICollection<View> minis)
		{
			MinisLayout.Children.Clear();
			minis.ForEach(m => MinisLayout.Children.Add(m));
		}

		protected override void OnDisappearing()
		{
			((GamePageViewModel)BindingContext).OnDisappearing();
			base.OnDisappearing();
		}
	}
}