using System.Threading.Tasks;

namespace DiceRoller.Interfaces
{
    public interface ICreatorView
    {
	    Task<bool> ImageSourceAlert(string imgType);
	}
}
