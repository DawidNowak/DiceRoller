using System.Threading.Tasks;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Helpers;
using DiceRoller.DataAccess.Models;
using DiceRoller.Interfaces;
using DiceRoller.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DiceRoller.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DiceCreatorPage : ContentPage, IDiceCreatorView
	{
		DiceCreatorPageViewModel _vm;
		
		public DiceCreatorPage ()
		{
			InitializeComponent ();
		}

	    protected override void OnBindingContextChanged()
	    {
			_vm = (DiceCreatorPageViewModel)BindingContext;
	        _vm.View = this;
	        base.OnBindingContextChanged();
	    }

	    public async Task<bool> ImageSourceAlert(string imgType)
	    {
	        return await DisplayAlert($"Set {imgType}", $"{Consts.PictureNote}. Which image source you want to use?", "File", "Camera");
        }

		public async Task DisplayPopup(string title, string msg, string cancel)
		{
			await DisplayAlert(title, msg, cancel);
		}

		public void AddWall(SwipeableImage wall)
		{
			wall.SwipedLeft += (sender, args) => RemoveWall((SwipeableImage)sender);
			wall.SwipedRight += (sender, args) => RemoveWall((SwipeableImage)sender);
			DiceWallLayout.Children.Add(wall);
		}

		private void RemoveWall(SwipeableImage o)
		{
			var wall = (DiceWall)o.BindingContext;
			var index = DiceWallLayout.Children.IndexOf(o);
			if (index < 0) return;
			DiceWallLayout.Children.RemoveAt(index);
			_vm.DeleteDiceWall(wall);
		}

		private void Refresh()
		{
			DiceWallLayout.Children.Clear();
			_vm.DiceWalls.ForEach(i => AddWall(i));
		}
	}
}