using System.Linq;
using DiceRoller.Interfaces;
using DiceRoller.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace DiceRoller.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamePage : ContentPage, IView
	{
		public GamePage ()
		{
			InitializeComponent ();
		}

	    protected override void OnBindingContextChanged()
	    {
	        ((GamePageViewModel) BindingContext).View = this;
            base.OnBindingContextChanged();
	    }

	    public void AddDice()
	    {
	        DiceWrapper.Children.Add(new Button {Text = "I'm added!"});
	    }

	    public void RemoveDice()
	    {
	        DiceWrapper.Children.RemoveAt(DiceWrapper.Children.Count - 1);
        }

	    public void PopulateMinis()
	    {
	        var ctx = (GamePageViewModel) BindingContext;
            var minis = ctx.DiceMinis;
	        var dice = ctx.Game.Dice.ToArray();

            for (int i = 0; i < minis.Count; i++)
	        {
	            var fullPath = $"{dice[i].Path}{minis[i]}";
                MinisWrapper.Children.Add(new Image
                {
                    Source = ImageSource.FromResource(fullPath),
                    BindingContext = dice[i]
                });
	        }
	    }
	}
}