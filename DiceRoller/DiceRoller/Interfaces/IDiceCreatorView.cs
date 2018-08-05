using DiceRoller.Controls;
using Xamarin.Forms;

namespace DiceRoller.Interfaces
{
    public interface IDiceCreatorView : ICreatorView
    {
	    void AddWall(SwipeableImage img);
    }
}
