using System.Collections.Generic;

namespace DiceRoller.DataAccess.Model
{
    public class Dice : Entity
    {
        public Dice(int walls, string path)
        {
            Walls = new DiceWall[walls];
            Path = path;
        }
        public ICollection<DiceWall> Walls { get; set; }
        public string Path { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
