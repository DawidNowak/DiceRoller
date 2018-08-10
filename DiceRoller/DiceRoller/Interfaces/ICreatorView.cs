using System.Threading.Tasks;

namespace DiceRoller.Interfaces
{
    public interface ICreatorView
    {
	    Task<bool> ImageSourceAlert(string imgType);
	    Task DisplayPopup(string title, string msg, string cancel);
    }
}
