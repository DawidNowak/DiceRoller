using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DiceRoller.DataAccess.Models
{
    [Table("Dice")]
    public class Dice : Entity
    {
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ICollection<DiceWall> Walls { get; set; }
        [MaxLength(50)]
        public string Path { get; set; }
        [MaxLength(50)]
        public string MiniImageSource { get; set; }
        [ForeignKey(typeof(Game))]
        public int GameId { get; set; }
        [ManyToOne]
        public virtual Game Game { get; set; }
        public bool IsGenerated { get; set; }
        public bool IsValid { get; set; }
		public byte[] MiniImage { get; set; }
    }
}
