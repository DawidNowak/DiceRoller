using Xamarin.Forms;

namespace DiceRoller.Interfaces
{
    public interface IMasterDetailsView
    {
        Page MasterPage { get; set; }
        Page DetailsPage { get; set; }
    }
}
