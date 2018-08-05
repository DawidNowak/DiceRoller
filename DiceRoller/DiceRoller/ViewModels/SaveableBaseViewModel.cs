using DiceRoller.DataAccess.Models;
using Prism.Commands;
using Prism.Navigation;

namespace DiceRoller.ViewModels
{
	public class SaveableBaseViewModel<T> : ViewModelBase where T : Entity
	{
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
	}
}
