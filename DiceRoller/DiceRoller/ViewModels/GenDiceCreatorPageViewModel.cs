using DiceRoller.DataAccess.Context;
using DiceRoller.DataAccess.Models;
using DiceRoller.Helpers;
using Prism.Navigation;
using Xamarin.Forms;

namespace DiceRoller.ViewModels
{
	public class GenDiceCreatorPageViewModel : SaveableBaseViewModel<Dice>
    {
		private readonly IContext _ctx;
	    public GenDiceCreatorPageViewModel(INavigationService navigationService, IContext ctx) : base(navigationService)
	    {
			_ctx = ctx;
		}

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
		    Model.MiniImage = DrawHelper.DrawDice(StartValue, StartValue, StartValue + WallsCount - 1).ToArray();
			_ctx.InsertOrReplace(Model);
		    await App.MasterDetail.Detail.Navigation.PopAsync();
		}

	    protected override bool canSave()
	    {
		    return WallsCount > 1;
	    }
    }
}
