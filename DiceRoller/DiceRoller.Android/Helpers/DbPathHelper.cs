using System;
using System.IO;
using DiceRoller.DataAccess.Context;

namespace DiceRoller.Droid.Helpers
{
    public class DbPathHelperDroid : IDbPathHelper
    {
        public string GetLocalFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "diceRollerDb.sqlite");
        }
    }
}