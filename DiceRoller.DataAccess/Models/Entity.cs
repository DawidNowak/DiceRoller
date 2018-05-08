using SQLite;

namespace DiceRoller.DataAccess.Models
{
    public abstract class Entity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
    }
}
