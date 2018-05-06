using System;
using System.IO;
using DiceRoller.DataAccess.Context;

namespace DiceRoller.iOS.Helpers
{
    public class DbPathHelperIOS : IDbPathHelper
    {
        public string GetLocalFilePath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library", "skills.db");
        }
    }
}