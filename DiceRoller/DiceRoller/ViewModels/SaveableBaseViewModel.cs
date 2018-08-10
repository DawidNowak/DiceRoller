using System.Threading.Tasks;
using DiceRoller.DataAccess.Models;
using DiceRoller.Interfaces;
using Prism.Commands;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
	public class SaveableBaseViewModel<T> : ViewModelBase where T : Entity
	{
		public ICreatorView View { get; set; }

		public SaveableBaseViewModel(INavigationService navigationService) : base(navigationService)
		{
			SaveCommand = new DelegateCommand(save, canSave);
		}

		private T _model;

		public T Model
		{
			get => _model;
			set => SetProperty(ref _model, value);
		}

		public DelegateCommand SaveCommand { get; set; }

		protected virtual void save()
		{
		}

		protected virtual bool canSave()
		{
			return true;
		}

		public virtual void SetModel(T model)
		{
			Model = model;
		}

		protected async Task PermissionDeniedPopup(string permissionType)
		{
			await View.DisplayPopup("Permission denied", $"Access to {permissionType} is denied, please update device settings",
				"OK");
		}
	}
}
