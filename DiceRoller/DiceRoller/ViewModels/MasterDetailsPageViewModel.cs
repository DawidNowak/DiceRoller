using DiceRoller.DataAccess.Context;
using DiceRoller.Helpers;
using DiceRoller.Interfaces;
using Prism.Ioc;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
    public class MasterDetailsPageViewModel : ViewModelBase
    {
        public IMasterDetailsView View { get; set; }
        public MasterDetailsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        public void BindPagesViewModels()
        {
            var ctx = App.GlobalContainer.Resolve<IContext>();
            var eventAggregator = App.GlobalContainer.Resolve<IEventAgregator>();

            var settingsViewModel = new SettingsPageViewModel(NavigationService, ctx, eventAggregator);
            View.MasterPage.BindingContext = settingsViewModel;

            var mainViewModel = new MainPageViewModel(NavigationService, ctx, eventAggregator);
            View.DetailsPage.BindingContext = mainViewModel;
        }
    }
}
