using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiceRoller.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SaveableBase : ContentView
	{
		public SaveableBase ()
		{
			InitializeComponent ();
		}

		public static BindableProperty AdditionalContentProperty
			= BindableProperty.Create(nameof(AdditionalContent), typeof(object), typeof(SaveableBase));

		public object AdditionalContent
		{
			get => GetValue(AdditionalContentProperty);
			set => SetValue(AdditionalContentProperty, value);
		}
	}
}