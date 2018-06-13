using DiceRoller.DataAccess.Context;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
    public class GameCreatorPageViewModel : ViewModelBase
    {
        private readonly IContext _ctx;

        public GameCreatorPageViewModel(IContext ctx, INavigationService navigationService) : base(navigationService)
        {
            _ctx = ctx;
        }

        public string Title { get; set; } = "Game Creator";
    }
}
