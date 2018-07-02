﻿using System.Threading.Tasks;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Helpers;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using DiceRoller.Interfaces;
using DiceRoller.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiceRoller.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DiceCreatorPage : ContentPage, IDiceCreatorView
	{
		public DiceCreatorPage ()
		{
			InitializeComponent ();
		}

	    protected override void OnBindingContextChanged()
	    {
	        ((DiceCreatorPageViewModel) BindingContext).View = this;
	        base.OnBindingContextChanged();
	    }

	    public async Task<bool> ImageSourceAlert()
	    {
	        return await DisplayAlert("Set Mini Image", $"{Consts.PictureNote}. Which image source you want to use?", "File", "Camera");
        }

		public void AddWall(SwipeableImage wall)
		{
			DiceWallLayout.Children.Add(wall);
		}
	}
}