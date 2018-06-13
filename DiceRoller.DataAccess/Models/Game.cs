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

        [MaxLength(50)]
        public string Path { get; set; }

        [MaxLength(50)]
        public string LogoImageSource { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public ICollection<Dice> Dice { get; set; }

        [MaxLength(70)]
        public string DiceSet { get; set; }

        public byte[] LogoImage { get; set; }
        public bool IsEditable { get; set; }
    }
}
