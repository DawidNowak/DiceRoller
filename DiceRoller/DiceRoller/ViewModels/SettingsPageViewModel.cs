using System;
using System.Linq;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Helpers;
using DiceRoller.DataAccess.Models;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {

        //TODO: REFACTOR THIS TO BE MORE REUSABLE FOR EACH CONFIG KEY
        private readonly IContext _ctx;
        private readonly Config _rollCfg;
        private readonly Config _saveStateCfg;

        public SettingsPageViewModel(INavigationService navigationService, IContext ctx) : base(navigationService)
        {
            _ctx = ctx;
            var configs = _ctx.GetAll<Config>();
            _rollCfg = configs.First(c => c.Key == Consts.RollAnimationKey);
            _saveStateCfg = configs.First(c => c.Key == Consts.SaveDiceStateKey);
            AnimateRoll = Convert.ToBoolean(_rollCfg.Value);
            SaveDiceState = Convert.ToBoolean(_saveStateCfg.Value);
        }


        private bool _animateRoll;

        public bool AnimateRoll
        {
            get => _animateRoll;
            set => SetProperty(ref _animateRoll, value);
        }

        private bool _saveState;
        public bool SaveDiceState
        {
            get => _saveState;
            set => SetProperty(ref _saveState, value);
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            _rollCfg.Value = _animateRoll.ToString();
            _saveStateCfg.Value = _saveState.ToString();
            _ctx.InsertOrReplace(_rollCfg);
            _ctx.InsertOrReplace(_saveStateCfg);
            base.OnNavigatedFrom(parameters);
        }
    }
}
