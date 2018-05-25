using System;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Helpers;
using DiceRoller.DataAccess.Models;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly IContext _ctx;
        private readonly Config _roll;

        public SettingsPageViewModel(INavigationService navigationService, IContext ctx) : base(navigationService)
        {
            _ctx = ctx;
            _roll = _ctx.GetByFirstOrDefault<Config>(c => c.Key == Consts.RollAnimationKey);
            AnimateRoll = Convert.ToBoolean(_roll.Value);
        }

        private bool _animateRoll;

        public bool AnimateRoll
        {
            get => _animateRoll;
            set => SetProperty(ref _animateRoll, value);
        }

        public override void OnNavigatedFrom(NavigationParameters parameters)
        {
            _roll.Value = _animateRoll.ToString();
            _ctx.InsertOrReplace(_roll);
            base.OnNavigatedFrom(parameters);
        }
    }
}
