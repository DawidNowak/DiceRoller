using System.Threading.Tasks;
using DiceRoller.Controls;
using DiceRoller.DataAccess.Models;

namespace DiceRoller.Interfaces
{
    public interface IDiceCreatorView
    {
        Task<bool> ImageSourceAlert();
	    void AddWall(SwipeableImage wall);
    }
}
