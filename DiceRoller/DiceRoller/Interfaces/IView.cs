using System.Collections.Generic;
using DiceRoller.DataAccess.Models;
using Xamarin.Forms;

namespace DiceRoller.Interfaces
{
    public interface IView
    {
        void AddDice(Dice mini);
        void RemoveDice(int index);
        void RefreshMinis(ICollection<View> minis);
    }
}
