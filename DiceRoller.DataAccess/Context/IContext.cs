using DiceRoller.DataAccess.Models;

namespace DiceRoller.DataAccess.Context
{
    public interface IContext
    {
        string Path { get; }
        void CreateTable<T>() where T : Entity, new();
        void InsertOrReplace(object o);
        T GetById<T>(int id, bool eagerLoading = true) where T : Entity, new();
    }
}
