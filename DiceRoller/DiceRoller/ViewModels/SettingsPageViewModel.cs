using System;
using System.Collections.Generic;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Helpers;
using DiceRoller.DataAccess.Models;
using DiceRoller.Extensions;
using Prism.Navigation;
using Xamarin.Forms.Internals;

namespace DiceRoller.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly IContext _ctx;
        private readonly IDictionary<string, Config> _configs;

        public SettingsPageViewModel(INavigationService navigationService, IContext ctx) : base(navigationService)
        {
            _ctx = ctx;
            _configs = new Dictionary<string, Config>();
            _ctx.GetAll<Config>().ForEach(cfg => { _configs[cfg.Key] = cfg; });
            AnimateRoll =_configs[Consts.RollAnimationKey].Value.ToBoolean();
            SaveDiceState = _configs[Consts.SaveDiceStateKey].Value.ToBoolean();
        }

        public bool AnimateRoll
        {
            get => _configs[Consts.RollAnimationKey].Value.ToBoolean();
            set
            {
                _configs[Consts.RollAnimationKey].Value = value.ToString();
                RaisePropertyChanged();
            }
        }

        public bool SaveDiceState
        {
            get => _configs[Consts.SaveDiceStateKey].Value.ToBoolean();
            set
            {
                _configs[Consts.SaveDiceStateKey].Value = value.ToString();
                RaisePropertyChanged();
            }
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            _configs.ForEach(pair => _ctx.InsertOrReplace(pair.Value));
            base.OnNavigatedFrom(parameters);
        }
    }
}
