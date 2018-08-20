using System;
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

		public override void SetModel(Dice model)
		{
			base.SetModel(model);
			var diceData = new DiceData(model.Path == "Generated" ? "d0_0." : model.Path);
			Path = model.Path;
			WallsCount = diceData.WallsCount;
			StartValue = diceData.StartValue;
		}

		protected override async void save()
		{
			Model.Path = $"d{WallsCount}_{StartValue}.";
			Model.MiniImage = DrawHelper.DrawDice(new DiceData(Model.Path), false).ToArray();
			RefreshGame?.Invoke();
			await App.MasterDetail.Detail.Navigation.PopAsync();
		}

		protected override bool canSave()
		{
			if (Model == null) return false;
			Model.IsValid = WallsCount > 1;
			return Model.IsValid;
		}
	}
}
