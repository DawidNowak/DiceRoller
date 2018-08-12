using System;
using System.Collections.Generic;
using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
	public class GenDiceCreatorPageViewModel : SaveableBaseViewModel<Dice>
    {
		private readonly IContext _ctx;
	    public GenDiceCreatorPageViewModel(INavigationService navigationService, IContext ctx) : base(navigationService)
	    {
			_ctx = ctx;
		}

	    public Action RefreshGame { get; set; }

		private string _path;

	    public string Path
	    {
		    get => _path;
		    set => SetProperty(ref _path, value);
	    }

	    private int _wallsCount;
	    public int WallsCount
	    {
		    get => _wallsCount;
		    set
		    {
			    SetProperty(ref _wallsCount, value);
			    SaveCommand.RaiseCanExecuteChanged();
		    }
	    }

		private int _startValue;
	    public int StartValue
	    {
		    get => _startValue;
		    set => SetProperty(ref _startValue, value);
	    }

	    protected override async void save()
	    {
		    Model.Path = $"d{WallsCount}_{StartValue}.";
		    Model.MiniImage = DrawHelper.DrawDice(new DiceData(Model.Path), false).ToArray();
			Model.Game.Dice = new List<Dice>(Model.Game.Dice) {Model}.ToArray();
			_ctx.InsertOrReplace(Model);
			RefreshGame?.Invoke();
		    await App.MasterDetail.Detail.Navigation.PopAsync();
		}

	    protected override bool canSave()
	    {
		    return WallsCount > 1;
	    }
    }
}
