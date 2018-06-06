using System;
using System.Collections.Generic;
using System.Linq;
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
            var rand = new Random();
            SwipeableImage diceImg;

            if (!mini.IsGenerated)
            {
                diceImg = new SwipeableImage
                {
                    Source = ImageSource.FromResource(((GamePageViewModel)BindingContext).Game.Path + mini.Path + mini.Walls.ElementAt(rand.Next(0, mini.Walls.Count)).ImageSource),
                    BindingContext = mini,
                    HeightRequest = 64d,
                    WidthRequest = 64d
                };
                Thread.Sleep(10);
            }
            else
            {
                var wallsCount = Convert.ToInt16(mini.Path.Substring(1, mini.Path.Length - 2));
                var skData = DrawHelper.DrawDice(rand.Next(1, wallsCount+1), wallsCount);

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
            if (index >= 0)
            {
                DiceLayout.Children.RemoveAt(index);
                ((GamePageViewModel)BindingContext).DiceNumber--;
            }
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