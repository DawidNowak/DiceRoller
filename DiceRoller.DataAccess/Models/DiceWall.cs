using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DiceRoller.DataAccess.Models
{
    [Table("DiceWalls")]
    public class DiceWall : Entity
    {
        [MaxLength(50)]
        public string ImageSource { get; set; }
        [ForeignKey(typeof(Dice))]
        public int DiceId { get; set; }
        [ManyToOne]
        public virtual Dice Dice { get; set; }

        public byte[] Image { get; set; }
    }
}
