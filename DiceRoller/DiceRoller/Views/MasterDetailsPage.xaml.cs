using DiceRoller.Interfaces;
using DiceRoller.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiceRoller.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterDetailsPage : IMasterDetailsView
	{
		public MasterDetailsPage ()
		{
			InitializeComponent();
            Master = new SettingsPage();
            Detail = new NavigationPage(new MainPage());
		    App.MasterDetail = this;
		}

	    protected override void OnBindingContextChanged()
	    {
	        var vm = (MasterDetailsPageViewModel) BindingContext;
            vm.View = this;
            base.OnBindingContextChanged();
            vm.BindPagesViewModels();
	    }

	    public Page MasterPage
	    {
	        get => Master;
	        set => Master = value;
	    }

	    public Page DetailsPage
	    {
	        get => Detail;
	        set => Detail = value;
	    }
	}
}