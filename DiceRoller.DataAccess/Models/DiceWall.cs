namespace DiceRoller.DataAccess.Model
{
    public class DiceWall : Entity
    {
        public string ImageSource { get; set; }
        public int DiceId { get; set; }
        public virtual Dice Dice { get; set; }
    }
}
