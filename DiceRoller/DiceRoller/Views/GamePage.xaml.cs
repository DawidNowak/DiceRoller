using System.Collections.Generic;
using System.Linq;
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

        public void AddDice(Dice mini)
        {
            var dice = new Image
            {
                Source = ImageSource.FromResource(mini.Path + mini.Walls.First().ImageSource),
                BindingContext = mini,
                Scale = 2.0
            };

            DiceLayout.Children.Add(dice);
        }

        public void RemoveDice(int index)
        {
            DiceLayout.Children.RemoveAt(index);
        }

        public void RefreshMinis(ICollection<View> minis)
        {
            MinisLayout.Children.Clear();
            minis.ForEach(m => MinisLayout.Children.Add(m));
        }
    }
}