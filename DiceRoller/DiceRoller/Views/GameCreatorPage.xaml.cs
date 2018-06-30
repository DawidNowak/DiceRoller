using System.Threading.Tasks;
using DiceRoller.DataAccess.Helpers;
using DiceRoller.Interfaces;
using DiceRoller.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiceRoller.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GameCreatorPage : ContentPage, IGameCreatorView
	{
		public GameCreatorPage()
		{
			InitializeComponent ();
		}

	    protected override void OnBindingContextChanged()
	    {
	        ((GameCreatorPageViewModel) BindingContext).View = this;
	        base.OnBindingContextChanged();
	    }

	    public async Task<bool> DisplayAlert(string diceName)
	    {
	        return await DisplayAlert("Confirm", $"Delete {diceName}?", "Yes", "No");
        }

		public async Task<bool> ImageSourceAlert()
		{
			return await DisplayAlert("Set Logo Image", $"{Consts.PictureNote}. Which image source you want to use?", "File", "Camera");
		}
	}
}