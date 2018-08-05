using Xamarin.Forms;

namespace DiceRoller.Triggers
{
    public class DigitsValidationTriggerAction : TriggerAction<Entry>
    {
	    private string _prevValue = string.Empty;

	    protected override void Invoke(Entry sender)
	    {
		    int number;
		    var isInt = int.TryParse(sender.Text, out number);

		    if (!string.IsNullOrWhiteSpace(sender.Text) && (sender.Text.Length > 3 || !isInt))
		    {
			    sender.Text = _prevValue;
			    return;
		    }

		    _prevValue = sender.Text;
		}
    }
}
