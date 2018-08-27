using Android.Content;
using Android.Graphics.Drawables;
using DiceRoller.Controls;
using DiceRoller.Droid.CustomRenderers;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(RoundedButton), typeof(RoundedButtonRenderer))]
namespace DiceRoller.Droid.CustomRenderers
{
	class RoundedButtonRenderer : ButtonRenderer
	{
		private GradientDrawable _normal, _pressed;
		public RoundedButtonRenderer(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			if(Control != null)
			{
				var button = (RoundedButton)e.NewElement;

				button.SizeChanged += (s, args) =>
				{
					var radius = (float)Math.Min(button.Width, button.Height);

					// Create a drawable for the button's normal state
					_normal = new GradientDrawable();

					if (button.BackgroundColor.R == -1.0 && button.BackgroundColor.G == -1.0 && button.BackgroundColor.B == -1.0)
						_normal.SetColor(Android.Graphics.Color.ParseColor("#ff2c2e2f"));
					else
						_normal.SetColor(button.BackgroundColor.ToAndroid());

					_normal.SetCornerRadius(radius);

					// Create a drawable for the button's pressed state
					_pressed = new Android.Graphics.Drawables.GradientDrawable();
					var highlight = Context.ObtainStyledAttributes(new int[] { Android.Resource.Attribute.ColorActivatedHighlight }).GetColor(0, Android.Graphics.Color.Gray);
					_pressed.SetColor(highlight);
					_pressed.SetCornerRadius(radius);

					// Add the drawables to a state list and assign the state list to the button
					var sld = new StateListDrawable();
					sld.AddState(new int[] { Android.Resource.Attribute.StatePressed }, _pressed);
					sld.AddState(new int[] { }, _normal);
#pragma warning disable CS0618 // Type or member is obsolete
					Control.SetBackgroundDrawable(sld);
#pragma warning restore CS0618 // Type or member is obsolete
				};
			}
		}
	}
}