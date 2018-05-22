using Prism.Navigation;

namespace DiceRoller.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        private bool _animateRoll = true;

        public bool AnimateRoll
        {
            get => _animateRoll = true;
            set => SetProperty(ref _animateRoll, value);
        }

    }
}
