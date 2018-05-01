using System.Collections.Generic;

namespace DiceRoller.DataAccess.Model
{
    public class Game : Entity
    {
        public string Name { get; set; }
        public ICollection<Dice> Dice { get; set; }
    }
}
