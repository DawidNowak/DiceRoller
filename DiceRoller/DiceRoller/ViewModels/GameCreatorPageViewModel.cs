using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using Prism.Commands;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
    public class GameCreatorPageViewModel : ViewModelBase
    {
        private readonly IContext _ctx;
        private readonly IEventAgregator _eventAggregator;

        public GameCreatorPageViewModel(IContext ctx, INavigationService navigationService, IEventAgregator eventAggregator) : base(navigationService)
        {
            _ctx = ctx;
            _eventAggregator = eventAggregator;
            SaveCommand = new DelegateCommand(Save, CanSave);
        }

        public DelegateCommand SaveCommand { get; set; }

        public string Title { get; set; } = "Game Creator";

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        private string _logoImgPath;

        public string LogoImgPath
        {
            get => _logoImgPath;
            set => SetProperty(ref _logoImgPath, value);
        }

        private void Save()
        {
            var game = new Game
            {
                Id = _ctx.GetNextId<Game>(),
                IsEditable = true,
                Name = Name
            };

            _ctx.InsertOrReplace(game);
            _eventAggregator.Publish<GameChangedEvent>();
        }

        private bool CanSave()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
