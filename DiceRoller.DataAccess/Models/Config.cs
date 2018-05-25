using SQLite;

namespace DiceRoller.DataAccess.Models
{
    [Table("Configs")]
    public class Config : Entity
    {
        [MaxLength(30)]
        public string Key { get; set; }
        [MaxLength(30)]
        public string Value { get; set; }
    }
}
