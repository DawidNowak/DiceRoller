using System.Collections.Generic;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Helpers;
using DiceRoller.DataAccess.Models;
using DiceRoller.Extensions;
using DiceRoller.Helpers;
using DiceRoller.Views;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace DiceRoller.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly IContext _ctx;
        private readonly IEventAgregator _eventAggregator;
        private readonly IDictionary<string, Config> _configs;

        public SettingsPageViewModel(INavigationService navigationService, IContext ctx, IEventAgregator eventAggregator) : base(navigationService)
        {
            _ctx = ctx;
            _eventAggregator = eventAggregator;
            _configs = new Dictionary<string, Config>();
            _ctx.GetAll<Config>().ForEach(cfg => { _configs[cfg.Key] = cfg; });
            NewGameCommand = new DelegateCommand(CreateNewGame);
        }

        public DelegateCommand NewGameCommand { get; set; }

        public bool AnimateRoll
        {
            get => _configs[Consts.RollAnimationKey].Value.ToBoolean();
            set
            {
                UpdateConfigKeyValue(Consts.RollAnimationKey, value.ToString());
                RaisePropertyChanged();
            }
        }

        public bool SaveDiceState
        {
            get => _configs[Consts.SaveDiceStateKey].Value.ToBoolean();
            set
            {
                UpdateConfigKeyValue(Consts.SaveDiceStateKey, value.ToString());
                RaisePropertyChanged();
            }
        }

        private void UpdateConfigKeyValue(string key, string value)
        {
            var cfg = _configs[key];
            cfg.Value = value;
            _ctx.InsertOrReplace(cfg);
        }

        private void CreateNewGame()
        {
            var vm = new GameCreatorPageViewModel(_ctx, NavigationService, _eventAggregator);
            var page = new GameCreatorPage { BindingContext = vm };

            App.MasterDetail.Detail.Navigation.PushAsync(page);
            App.MasterDetail.IsPresented = false;
        }
    }
}
