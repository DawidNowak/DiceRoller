using System;
using Android.Content;
using Android.Views;
using DiceRoller.Controls;
using DiceRoller.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SwipeableImage), typeof(SwipeableDroidImageRenderer))]
namespace DiceRoller.Droid.CustomRenderers
{
    public class SwipeableDroidImageRenderer : ImageRenderer
    {
        public SwipeableDroidImageRenderer(Context context) : base(context) {}

        public float X1 { get; set; }
        public float X2 { get; set; }
        public float Y1 { get; set; }
        public float Y2 { get; set; }

        public SwipeableImage SwipeableImage { get; set; }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (e.ActionMasked == MotionEventActions.Down)
            {
                X1 = e.GetX();
                return true;
            }

            X2 = e.GetX();

            var xChangeSize = Math.Abs(X1 - X2);

            if (xChangeSize > 100)
            {
                // horizontal
                if (X1 > X2)
                {
                    // left
                    SwipeableImage.RaiseSwipedLeft();
                }
                else
                {
                    // right
                    SwipeableImage.RaiseSwipedRight();
                }
            }
            try
            {
                return base.OnTouchEvent(e);
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Image> ev)
        {
            base.OnElementChanged(ev);

            SwipeableImage = (SwipeableImage)ev.NewElement;
        }

    }
}