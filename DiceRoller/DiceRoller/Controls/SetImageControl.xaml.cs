using Prism.Commands;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiceRoller.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SetImageControl : Grid
	{
		public SetImageControl()
		{
			InitializeComponent();
		}

		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(nameof(Text), typeof(string), typeof(SetImageControl));

		public string Text
		{
			get => (string) GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public static readonly BindableProperty ImageSourceProperty =
			BindableProperty.Create(nameof(ImageSource), typeof(ImageSource), typeof(SetImageControl));

		public ImageSource ImageSource
		{
			get => (ImageSource) GetValue(ImageSourceProperty);
			set => SetValue(ImageSourceProperty, value);
		}

		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(nameof(Command), typeof(DelegateCommand), typeof(SetImageControl));

		public DelegateCommand Command
		{
			get => (DelegateCommand) GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}
	}
}