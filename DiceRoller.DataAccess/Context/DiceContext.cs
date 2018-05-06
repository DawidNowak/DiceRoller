using DiceRoller.DataAccess.Models;
using SQLite;

namespace DiceRoller.DataAccess.Context
{
    public class DiceContext : IContext
    {
        public DiceContext(IDbPathHelper helper)
        {
            Path = helper.GetLocalFilePath();
            _conn = new SQLiteConnection(Path);
        }

        private readonly SQLiteConnection _conn;
        public string Path { get; }

        public void InsertOrReplace(object o)
        {
            _conn.InsertOrReplace(o);
        }

        public void CreateTable<T>() where T : Entity, new()
        {
            _conn.CreateTable<T>();
        }
    }
}
