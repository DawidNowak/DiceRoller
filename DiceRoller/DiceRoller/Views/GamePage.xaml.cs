using System;
using System.Collections.Generic;
using System.Linq;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Models;
using DiceRoller.Interfaces;
using DiceRoller.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace DiceRoller.Views
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage, IView
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
            var diceImg = new SwipeableImage
            {
                Source = ImageSource.FromResource(mini.Path + mini.Walls.ElementAt(rand.Next(0, mini.Walls.Count)).ImageSource),
                BindingContext = mini,
                HeightRequest = 64d,
                WidthRequest = 64d
            };

            diceImg.SwipedLeft += (sender, args) => RemoveDice(DiceLayout.Children.IndexOf(sender));
            diceImg.SwipedRight += (sender, args) => RemoveDice(DiceLayout.Children.IndexOf(sender));

            DiceLayout.Children.Add(diceImg);
        }

        public void RemoveDice(int index)
        {
            if (index >= 0)
            {
                DiceLayout.Children.RemoveAt(index);
                ((GamePageViewModel) BindingContext).DiceNumber--;
            }
        }

        public void RefreshMinis(ICollection<View> minis)
        {
            MinisLayout.Children.Clear();
            minis.ForEach(m => MinisLayout.Children.Add(m));
        }
    }
}