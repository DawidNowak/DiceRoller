using System;
using DiceRoller.Controls;
using DiceRoller.iOS.CustomRenderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SwipeableImage), typeof(SwipeableIosImageRenderer))]
namespace DiceRoller.iOS.CustomRenderers
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);
    public class SwipeableIosImageRenderer : ImageRenderer
    {
        UISwipeGestureRecognizer _swipeLeftGestureRecognizer;
        UISwipeGestureRecognizer _swipeRightGestureRecognizer;


        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            _swipeLeftGestureRecognizer = new UISwipeGestureRecognizer(() =>
            {
                var picture = (SwipeableImage) e.NewElement;
                if (_swipeLeftGestureRecognizer.Direction == UISwipeGestureRecognizerDirection.Left)
                    picture.RaiseSwipedLeft();
            }) {Direction = UISwipeGestureRecognizerDirection.Left};

            _swipeRightGestureRecognizer = new UISwipeGestureRecognizer(() =>
            {
                var picture = (SwipeableImage) e.NewElement;
                if (_swipeRightGestureRecognizer.Direction == UISwipeGestureRecognizerDirection.Right)
                    picture.RaiseSwipedRight();
            }) {Direction = UISwipeGestureRecognizerDirection.Right};

            if (e.NewElement == null)
            {
                if (_swipeLeftGestureRecognizer != null)
                {
                    RemoveGestureRecognizer(_swipeLeftGestureRecognizer);
                }
                if (_swipeRightGestureRecognizer != null)
                {
                    RemoveGestureRecognizer(_swipeRightGestureRecognizer);
                }
            }

            if (e.OldElement != null) return;
            AddGestureRecognizer(_swipeLeftGestureRecognizer);
            AddGestureRecognizer(_swipeRightGestureRecognizer);
        }
    }
}