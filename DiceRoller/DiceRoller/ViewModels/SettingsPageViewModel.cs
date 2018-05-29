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
        }

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
    }
}
