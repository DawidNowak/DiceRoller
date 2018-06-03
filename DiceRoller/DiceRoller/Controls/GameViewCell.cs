using DiceRoller.DataAccess.Models;
using Prism.Commands;
using Xamarin.Forms;

namespace DiceRoller.Controls
{
    public class GameViewCell : ViewCell
    {
        public static BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(Game), typeof(GameViewCell));

        public Game CommandParameter
        {
            get => (Game)GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(DelegateCommand<Game>), typeof(GameViewCell));

        public DelegateCommand<Game> Command
        {
            get => (DelegateCommand<Game>)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        protected override void OnTapped()
        {
            base.OnTapped();
            Command?.Execute(CommandParameter);
        }
    }
}