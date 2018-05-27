using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DiceRoller.DataAccess.Models
{
    [Table("Games")]
    public class Game : Entity
    {
        [MaxLength(70)]
        public string Name { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ICollection<Dice> Dice { get; set; }

        [MaxLength(70)]
        public string DiceSet { get; set; }
    }
}
