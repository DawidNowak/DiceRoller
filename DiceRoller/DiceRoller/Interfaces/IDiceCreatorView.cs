using System.Threading.Tasks;

namespace DiceRoller.Interfaces
{
    public interface IDiceCreatorView
    {
        Task<bool> ImageSourceAlert();
    }
}
