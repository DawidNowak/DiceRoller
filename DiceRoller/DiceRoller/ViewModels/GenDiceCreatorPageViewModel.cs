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

	    private ImageSource _miniImageSource;

	    public ImageSource MiniImageSource
	    {
		    get => _miniImageSource;
		    set => SetProperty(ref _miniImageSource, value);
	    }

		public override void SetModel(Dice model)
	    {
		    base.SetModel(model);
		    if (Model.MiniImage != null) MiniImageSource = BlobHelper.GetImgSource(Model.MiniImage);
		}

	    protected override async void save()
	    {
		    Model.Path = $"d{WallsCount}_{StartValue}.";
			_ctx.InsertOrReplace(Model);
		    await App.MasterDetail.Detail.Navigation.PopAsync();
		}

	    protected override bool canSave()
	    {
		    return WallsCount > 1;
	    }
    }
}
