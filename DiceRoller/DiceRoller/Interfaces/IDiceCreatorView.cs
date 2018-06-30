using System.Threading.Tasks;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Models;

namespace DiceRoller.Interfaces
{
    public interface IDiceCreatorView : ICreatorView
    {
	    void AddWall(SwipeableImage wall);
    }
}
