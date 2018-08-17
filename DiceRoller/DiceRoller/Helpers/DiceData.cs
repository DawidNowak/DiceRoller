using System;

namespace DiceRoller.Helpers
{
	public class DiceData
    {
	    public int WallsCount { get; set; }
	    public int StartValue { get; set; }

	    public DiceData(string info)
	    {
		    var sep = info.Split('_');
		    WallsCount = Convert.ToInt32(sep[0].Substring(1));
		    StartValue = Convert.ToInt32(sep[1].Split('.')[0]);
	    }
    }
}
