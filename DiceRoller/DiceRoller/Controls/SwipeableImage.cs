using System;
using Xamarin.Forms;

namespace DiceRoller.Controls
{
    public class SwipeableImage : Image
    {
        public event EventHandler SwipedLeft;
        public event EventHandler SwipedRight;

        public void RaiseSwipedLeft()
        {
            SwipedLeft?.Invoke(this, new EventArgs());
        }

        public void RaiseSwipedRight()
        {
            SwipedRight?.Invoke(this, new EventArgs());
        }
    }
}
