using System;
using System.Collections.Generic;
using System.Text;

namespace DiceRoller.Extensions
{
    public static class StringExtensions
    {
        public static bool ToBoolean(this string @in)
        {
            return @in == "True" || @in == "true";
        }
    }
}
