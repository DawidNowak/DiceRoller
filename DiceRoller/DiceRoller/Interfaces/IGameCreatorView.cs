using System.Threading.Tasks;

namespace DiceRoller.Interfaces
{
    public interface IGameCreatorView
    {
        Task<bool> DisplayAlert(string diceName);
    }
}
