using System.Threading.Tasks;

namespace DiceRoller.Interfaces
{
    public interface IGameCreatorView : ICreatorView
    {
        Task<bool> DisplayAlert(string diceName);
	    Task<bool> DiceTypeAlert();
    }
}
